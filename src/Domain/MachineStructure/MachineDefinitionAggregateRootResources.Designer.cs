﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MicroDotNet.Services.StateMachines.Domain.MachineStructure {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class MachineDefinitionAggregateRootResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal MachineDefinitionAggregateRootResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MicroDotNet.Services.StateMachines.Domain.MachineStructure.MachineDefinitionAggre" +
                            "gateRootResources", typeof(MachineDefinitionAggregateRootResources).Assembly);
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
        ///   Looks up a localized string similar to Nodes to be added array is empty.
        /// </summary>
        internal static string AddNodes_NodesToAddEmpty {
            get {
                return ResourceManager.GetString("AddNodes_NodesToAddEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Source node &apos;{0}&apos; does not exist..
        /// </summary>
        internal static string AddTransition_SourceNodeDoesNotExist {
            get {
                return ResourceManager.GetString("AddTransition_SourceNodeDoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Target node &apos;{0}&apos; does not exist..
        /// </summary>
        internal static string AddTransition_TargetNodeDoesNotExist {
            get {
                return ResourceManager.GetString("AddTransition_TargetNodeDoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Transition already exists between nodes &apos;{0}&apos; and &apos;{1}&apos; for trigger &apos;{2}&apos;..
        /// </summary>
        internal static string AddTransition_TransitionAlreadyExists {
            get {
                return ResourceManager.GetString("AddTransition_TransitionAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Trigger must not be empty..
        /// </summary>
        internal static string AddTransition_TriggerMustNotBeEmpty {
            get {
                return ResourceManager.GetString("AddTransition_TriggerMustNotBeEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Machine is not being designed and cannot be confirmed..
        /// </summary>
        internal static string Confirm_NotInDesign {
            get {
                return ResourceManager.GetString("Confirm_NotInDesign", resourceCulture);
            }
        }
    }
}
