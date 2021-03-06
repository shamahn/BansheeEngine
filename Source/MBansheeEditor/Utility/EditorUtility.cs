﻿//********************************** Banshee Engine (www.banshee3d.com) **************************************************//
//**************** Copyright (c) 2016 Marko Pintera (marko.pintera@gmail.com). All rights reserved. **********************//
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BansheeEngine;

namespace BansheeEditor
{
    /** @addtogroup Utility-Editor
     *  @{
     */

    /// <summary>
    /// Contains various utility methods used throughout the editor.
    /// </summary>
    public class EditorUtility
    {
        /// <summary>
        /// Calculates the axis aligned box that encapsulated the provided scene object and its components and children. 
        /// Only certain components like <see cref="Renderable"/> will be included in bound calculations.
        /// </summary>
        /// <param name="so">Scene object to calculate the bounds for.</param>
        /// <returns>Axis aligned box encompassing all scene object elements, in world space.</returns>
        public static AABox CalculateBounds(SceneObject so)
        {
            AABox bounds;
            Internal_CalculateBounds(so, out bounds);
            return bounds;
        }

        /// <summary>
        /// Calculates the axis aligned box that encapsulated the provided scene objects and their components and children. 
        /// Only certain components like <see cref="Renderable"/> will be included in bound calculations.
        /// </summary>
        /// <param name="objects">Scene objects to calculate the bounds for.</param>
        /// <returns>Axis aligned box encompassing all scene object elements, in world space.</returns>
        public static AABox CalculateBounds(SceneObject[] objects)
        {
            AABox bounds;
            Internal_CalculateBoundsArray(objects, out bounds);
            return bounds;
        }

        /// <summary>
        /// Converts a hierarchy of scene objects and their children into a flat array. Doesn't modify the scene object's
        /// themselves. 
        /// </summary>
        /// <param name="so">Scene object whose hierarchy to flatten.</param>
        /// <returns>Array of flattened hierarchy in a depth-first manner.</returns>
        public static SceneObject[] FlattenHierarchy(SceneObject so)
        {
            Stack<SceneObject> todo = new Stack<SceneObject>();
            todo.Push(so);

            List<SceneObject> flattenedHierarchy = new List<SceneObject>();
            while (todo.Count > 0)
            {
                SceneObject cur = todo.Pop();
                flattenedHierarchy.Add(cur);

                int numChildren = cur.GetNumChildren();
                for (int i = 0; i < numChildren; i++)
                    todo.Push(cur.GetChild(i));
            }

            return flattenedHierarchy.ToArray();
        }

        /// <summary>
        /// Find all resources that the provided resource depends on.
        /// </summary>
        /// <param name="resource">Resource whose dependencies to find.</param>
        /// <param name="recursive">Determines whether or not child objects will also be searched (if object has any 
        ///                         children).</param>
        /// <returns>All resources the provided resource is dependant on (does not include itself).</returns>
        public static Resource[] FindDependencies(Resource resource, bool recursive = true)
        {
            return Internal_FindDependencies(resource, recursive);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_CalculateBounds(SceneObject so, out AABox bounds);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_CalculateBoundsArray(SceneObject[] objects, out AABox bounds);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern Resource[] Internal_FindDependencies(Resource resource, bool recursive);
    }

    /** @} */
}
