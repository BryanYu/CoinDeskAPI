﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CodeDesk.Service.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resource_en_US {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource_en_US() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CodeDesk.Service.Resources.Resource.en-US", typeof(Resource_en_US).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed.
        /// </summary>
        internal static string ApiResponseStatus_Error {
            get {
                return ResourceManager.GetString("ApiResponseStatus_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Internal Server Error.
        /// </summary>
        internal static string ApiResponseStatus_InternalServerError {
            get {
                return ResourceManager.GetString("ApiResponseStatus_InternalServerError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Model Valid Fail.
        /// </summary>
        internal static string ApiResponseStatus_ModelValidError {
            get {
                return ResourceManager.GetString("ApiResponseStatus_ModelValidError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Success.
        /// </summary>
        internal static string ApiResponseStatus_Success {
            get {
                return ResourceManager.GetString("ApiResponseStatus_Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Euro Dollars.
        /// </summary>
        internal static string Currency_EUR {
            get {
                return ResourceManager.GetString("Currency_EUR", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pound.
        /// </summary>
        internal static string Currency_GBP {
            get {
                return ResourceManager.GetString("Currency_GBP", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to US Dollars.
        /// </summary>
        internal static string Currency_USD {
            get {
                return ResourceManager.GetString("Currency_USD", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CurrencyCode Max Length is 3.
        /// </summary>
        internal static string ModelValidError_CurrencyCodeLenghtMax3 {
            get {
                return ResourceManager.GetString("ModelValidError_CurrencyCodeLenghtMax3", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CurrencyCode Required.
        /// </summary>
        internal static string ModelValidError_CurrencyCodeRequired {
            get {
                return ResourceManager.GetString("ModelValidError_CurrencyCodeRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CurrencyName Max Length is 20.
        /// </summary>
        internal static string ModelValidError_CurrencyNameLengthMax20 {
            get {
                return ResourceManager.GetString("ModelValidError_CurrencyNameLengthMax20", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CurrencyName Required.
        /// </summary>
        internal static string ModelValidError_CurrencyNameRequired {
            get {
                return ResourceManager.GetString("ModelValidError_CurrencyNameRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PageNumber Must Be Greater Than Zero.
        /// </summary>
        internal static string ModelValidError_PageNumberMustBeGreaterThanZero {
            get {
                return ResourceManager.GetString("ModelValidError_PageNumberMustBeGreaterThanZero", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PageSize Can&apos;t Over Than {0}.
        /// </summary>
        internal static string ModelValidError_PageSizeCantOverSetting {
            get {
                return ResourceManager.GetString("ModelValidError_PageSizeCantOverSetting", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PageSize Must Be Greater Than Zero.
        /// </summary>
        internal static string ModelValidError_PageSizeMustBeGreaterThanZero {
            get {
                return ResourceManager.GetString("ModelValidError_PageSizeMustBeGreaterThanZero", resourceCulture);
            }
        }
    }
}
