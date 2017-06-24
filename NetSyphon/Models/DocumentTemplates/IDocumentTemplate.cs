using System.Collections.Generic;

namespace NetSyphon.Models.DocumentTemplates
{
    /// <summary>
    /// A contract for a DocumentTemplate builder
    /// </summary>
    public interface IDocumentTemplate
    {
        /// <summary>
        /// The template builder name, as specified in the JobDescription
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Builds an output document from a source object, according to a set of rules
        /// </summary>
        /// <param name="src">The source object</param>
        /// <returns></returns>
        object Build(object src, IDictionary<string, object> templateData);
    }
}