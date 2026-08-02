/**
 * Converts a string title into a URL-friendly slug.
 * Example: "sunt aut facere repeat" -> "sunt-aut-facere-repeat"
 */
export const slugify = (text: string): string => {
  return text
    .toString()
    .toLowerCase()
    .trim()
    .replace(/\s+/g, '-')        // Replace spaces with -
    .replace(/[^\w\-]+/g, '')    // Remove all non-word chars
    .replace(/\-\-+/g, '-');     // Replace multiple - with single -
};
