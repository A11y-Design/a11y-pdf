//  a11y pdf – A customizable PDF export tool for generating PDF files 
//  that meet the PDF/UA accessibility standard.
//  Copyright (C) 2025 a11y design GmbH, see <https://www.a11y-design.de/>.
//  This file is part of a11y pdf.
//
//  a11y pdf is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as
//  published by the Free Software Foundation, either version 3 of the
//  License, or (at your option) any later version.
//
//  a11y pdf is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY

using System;
using System.Collections.Generic;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Tagging;
using iText.Kernel.Pdf.Tagutils;


namespace A11y_Design_PowerPointAddin.Core
{
    internal static class TagTreePointerExtensions
    {
        public static class TempRoles
        {
            public const string MCRTemp = "MCRTemp";
        }
        public static string GetStandardRole(this TagTreePointer tagTreePointer)
        {
            var tagStructureContext = tagTreePointer.GetContext();
            return tagStructureContext.ResolveMappingToStandardOrDomainSpecificRole(tagTreePointer.GetRole(), tagTreePointer.GetNamespaceForNewTags())?.GetRole() ?? tagTreePointer.GetRole();
        }

        public static string GetStandardRoleOfKid(this TagTreePointer tagTreePointer, int kidsIndex)
        {
            var tagStructureContext = tagTreePointer.GetContext();
            return tagStructureContext.ResolveMappingToStandardOrDomainSpecificRole(tagTreePointer.GetKidsRoles()[kidsIndex], tagTreePointer.GetNamespaceForNewTags())?.GetRole() ?? tagTreePointer.GetKidsRoles()[kidsIndex];
        }

        public static List<PdfMcr> FindAllMcrStructureNodes(this TagTreePointer tagTreePointer)
        {
            
            var mcrStructureNodes = new List<PdfMcr>();
            for (int kidIndex = 0; kidIndex < tagTreePointer.GetKidsRoles().Count; kidIndex++)
            {

                var kidsRole = tagTreePointer.GetStandardRoleOfKid(kidIndex);

                if (kidsRole.Equals(PdfName.MCR.GetValue(), StringComparison.Ordinal))
                {
                    mcrStructureNodes.Add(tagTreePointer.GetPdfStructureElem().GetKids()[kidIndex] as PdfMcr);
                    continue;
                }

                mcrStructureNodes.AddRange(tagTreePointer.MoveToKid(kidIndex).FindAllMcrStructureNodes());
            }

            if (!tagTreePointer.GetStandardRole().Equals(PdfName.Table.GetValue(), StringComparison.Ordinal))
                tagTreePointer.MoveToParent();

            return mcrStructureNodes;
        }

        /// <summary>
        /// Removes tag and all its decendents (needed since flushtag does not work if any tags are in waiting state)
        /// </summary>
        /// <param name="tagTreePointer"></param>
        public static void RemoveTagRecursive(this TagTreePointer tagTreePointer)
        {
            for (int kidIndex = tagTreePointer.GetKidsRoles().Count - 1; kidIndex >= 0; kidIndex--)
            {
                var kidsRole = tagTreePointer.GetKidsRoles()[kidIndex];
                if (kidsRole.Equals(PdfName.MCR.GetValue(), StringComparison.Ordinal))
                {
                    tagTreePointer.RemoveTag();
                    return;
                }
                tagTreePointer.MoveToKid(kidIndex).RemoveTagRecursive();
            }

            tagTreePointer.RemoveTag();
        }

        public static void RemoveMcrTempParentRecursiv(this TagTreePointer tagTreePointer)
        {
            for (int kidIndex = tagTreePointer.GetKidsRoles().Count - 1; kidIndex >= 0; kidIndex--)
            {
                var kidsRole = tagTreePointer.GetKidsRoles()[kidIndex];
                if (kidsRole.Equals(TempRoles.MCRTemp, StringComparison.Ordinal))
                {
                    tagTreePointer.RemoveTag();
                    return;
                }
                tagTreePointer.MoveToKid(kidIndex).RemoveMcrTempParentRecursiv();
            }

            tagTreePointer.RemoveTag();
        }

        public static PdfStructElem GetPdfStructureElem(this TagTreePointer tagTreePointer) => tagTreePointer.GetContext().GetPointerStructElem(tagTreePointer);

        public static int GetKidsCount(this TagTreePointer tagTreePointer) => tagTreePointer.GetKidsRoles().Count;

        public static int GetMcid(this TagTreePointer tagTreePointer, int kidIndex)
        {
            if (tagTreePointer.GetPdfStructureElem().GetKids()[kidIndex] is PdfMcrNumber pdfMcrNumber)
            {
                return pdfMcrNumber.GetMcid();
            }
            if (tagTreePointer.GetPdfStructureElem().GetKids()[kidIndex] is PdfMcrDictionary pdfMcrDictionary)
            {
                return pdfMcrDictionary.GetMcid();
            }

            return -1;
        }


        /// <summary>
        /// Check if any child of the treepointer fullfills the given condition (like any from Linq)
        /// </summary>
        /// <param name="tagTreePointer"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static bool AnyRecurse(this TagTreePointer tagTreePointer, Func<TagTreePointer, bool> condition)
        {
            // check condition for current pointer
            if (condition.Invoke(tagTreePointer))
            {
                return true;
            }
            //check condition for kids
            else
            {
                bool sucess = false;
                for (int i = 0; i < tagTreePointer.GetKidsCount(); i++)
                {
                    //skip if role in unknown or marked content
                    var kid_role = tagTreePointer.GetKidsRoles()[i];
                    if (kid_role == null || kid_role.Equals("MCR"))
                        continue;

                    tagTreePointer.MoveToKid(i);
                    sucess = AnyRecurse(tagTreePointer, condition);
                    tagTreePointer.MoveToParent();


                    if (sucess) return true; // abort if found (but after going back to parrent)

                }
                return false;
            }
        }

        public static TagTreePointer GetKidAsTagTreePointer(this TagTreePointer tagTreePointer, int kidIndex) => new TagTreePointer(tagTreePointer).MoveToKid(kidIndex);

        /// <summary>
        /// Moves Treepointer to child temporary.
        /// <para>this should be called with a USING statement</para>
        /// </summary>
        /// <param name="tagTreePointer"></param>
        /// <param name="kidIndex"></param>
        /// <returns></returns>
        public static SafePointerContext MoveToKidSafe(this TagTreePointer tagTreePointer, int kidIndex)
        {
            tagTreePointer.MoveToKid(kidIndex);
            return new SafePointerContext(tagTreePointer, -1);
        }

        public static SafePointerContext MoveToParentSafe(this TagTreePointer tagTreePointer)
        {
            int kidindex = tagTreePointer.GetIndexInParentKidsList();
            tagTreePointer.MoveToParent();
            return new SafePointerContext(tagTreePointer, kidindex);
        }
    }

    /// <summary>
    /// This object is created when using safe move operations. Whenever object this gets out of scope (i.e. after the brackets) the pointer will be moved back to its origin. 
    /// </summary>
    public class SafePointerContext : IDisposable
    {
        TagTreePointer TagTreePointer;

        int kid_index;

        public SafePointerContext(TagTreePointer TagTreePointer, int traverse_dir)
        {
            this.TagTreePointer = TagTreePointer;
            this.kid_index = traverse_dir;
        }

        public void Dispose()
        {
            if (kid_index == -1)
                TagTreePointer.MoveToParent();
            else
                TagTreePointer.MoveToKid(kid_index);
        }
    }
}
