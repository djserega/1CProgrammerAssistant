namespace _1CProgrammerAssistant
{
    internal class AssistantObjects
    {
        internal static MethodStore.EF.MethodStoreContext MethodStoreContext = new MethodStore.EF.MethodStoreContext();
        
        internal DescriptionsTheMethods.Main DescriptionsTheMethodsMain { get; } = new DescriptionsTheMethods.Main();
        internal QueryParameters.Main QueryParametersMain { get; } = new QueryParameters.Main();
        internal MethodStore.Main MethodStoreMain { get; } = new MethodStore.Main();
        internal ModifiedFiles.Main ModifiedFilesMain { get; } = new ModifiedFiles.Main();
        internal ViewerFiles.Main ViewerFilesMain { get; } = new ViewerFiles.Main();
        internal MakingCode.Main MakingCodeMain { get; } = new MakingCode.Main();
    }
}
