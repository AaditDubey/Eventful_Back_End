using System.ComponentModel;

namespace Time.Commerce.Domains.Entities.Catalog
{
    /// <summary>
    /// Represents an attribute control type
    /// </summary>
    public enum AttributeControlType
    {
        /// <summary>
        /// Dropdown list
        /// </summary>
        [Description("Dropdown list")]
        DropdownList = 1,
        /// <summary>
        /// Radio list
        /// </summary>
        [Description("Radio list")]
        RadioList = 2,
        /// <summary>
        /// Checkboxes
        /// </summary>
        [Description("Checkboxes")]
        Checkboxes = 3,
        /// <summary>
        /// TextBox
        /// </summary>
        //[Description("TextBox")]
        //TextBox = 4,
        /// <summary>
        /// Multiline textbox
        /// </summary>
        [Description("Multiline text box")]
        MultilineTextbox = 10,
        /// <summary>
        /// Datepicker
        /// </summary>
        //[Description("Datepicker")]
        //Datepicker = 20,
        /// <summary>
        /// File upload control
        /// </summary>
        //[Description("File upload")]
        //FileUpload = 30,
        /// <summary>
        /// Color squares
        /// </summary>
        [Description("Color squares")]
        ColorSquares = 40,
        /// <summary>
        /// Image squares
        /// </summary>
        //[Description("Image squares")]
        //ImageSquares = 45,
        /// <summary>
        /// Read-only checkboxes
        /// </summary>
        //[Description("Readonly Checkboxes")]
        //ReadonlyCheckboxes = 50,
    }
}
