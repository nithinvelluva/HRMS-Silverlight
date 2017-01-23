﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.ServiceReference, version 5.0.61118.0
// 
namespace HRMSWEBAP.MailReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="", ConfigurationName="MailReference.MailService")]
    public interface MailService {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="urn:MailService/SendEmail", ReplyAction="urn:MailService/SendEmailResponse")]
        System.IAsyncResult BeginSendEmail(string UserName, string UserEmail, string Message, string imgAttachment, System.AsyncCallback callback, object asyncState);
        
        bool EndSendEmail(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="urn:MailService/SendQueryMail", ReplyAction="urn:MailService/SendQueryMailResponse")]
        System.IAsyncResult BeginSendQueryMail(string Username, string userEmail, string subject, string message, System.AsyncCallback callback, object asyncState);
        
        bool EndSendQueryMail(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface MailServiceChannel : HRMSWEBAP.MailReference.MailService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SendEmailCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public SendEmailCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public bool Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((bool)(results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SendQueryMailCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public SendQueryMailCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public bool Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((bool)(results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MailServiceClient : System.ServiceModel.ClientBase<HRMSWEBAP.MailReference.MailService>, HRMSWEBAP.MailReference.MailService {
        
        private BeginOperationDelegate onBeginSendEmailDelegate;
        
        private EndOperationDelegate onEndSendEmailDelegate;
        
        private System.Threading.SendOrPostCallback onSendEmailCompletedDelegate;
        
        private BeginOperationDelegate onBeginSendQueryMailDelegate;
        
        private EndOperationDelegate onEndSendQueryMailDelegate;
        
        private System.Threading.SendOrPostCallback onSendQueryMailCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public MailServiceClient() {
        }
        
        public MailServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MailServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MailServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MailServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Net.CookieContainer CookieContainer {
            get {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    return httpCookieContainerManager.CookieContainer;
                }
                else {
                    return null;
                }
            }
            set {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    httpCookieContainerManager.CookieContainer = value;
                }
                else {
                    throw new System.InvalidOperationException("Unable to set the CookieContainer. Please make sure the binding contains an HttpC" +
                            "ookieContainerBindingElement.");
                }
            }
        }
        
        public event System.EventHandler<SendEmailCompletedEventArgs> SendEmailCompleted;
        
        public event System.EventHandler<SendQueryMailCompletedEventArgs> SendQueryMailCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult HRMSWEBAP.MailReference.MailService.BeginSendEmail(string UserName, string UserEmail, string Message, string imgAttachment, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginSendEmail(UserName, UserEmail, Message, imgAttachment, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        bool HRMSWEBAP.MailReference.MailService.EndSendEmail(System.IAsyncResult result) {
            return base.Channel.EndSendEmail(result);
        }
        
        private System.IAsyncResult OnBeginSendEmail(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string UserName = ((string)(inValues[0]));
            string UserEmail = ((string)(inValues[1]));
            string Message = ((string)(inValues[2]));
            string imgAttachment = ((string)(inValues[3]));
            return ((HRMSWEBAP.MailReference.MailService)(this)).BeginSendEmail(UserName, UserEmail, Message, imgAttachment, callback, asyncState);
        }
        
        private object[] OnEndSendEmail(System.IAsyncResult result) {
            bool retVal = ((HRMSWEBAP.MailReference.MailService)(this)).EndSendEmail(result);
            return new object[] {
                    retVal};
        }
        
        private void OnSendEmailCompleted(object state) {
            if ((SendEmailCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                SendEmailCompleted(this, new SendEmailCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void SendEmailAsync(string UserName, string UserEmail, string Message, string imgAttachment) {
            SendEmailAsync(UserName, UserEmail, Message, imgAttachment, null);
        }
        
        public void SendEmailAsync(string UserName, string UserEmail, string Message, string imgAttachment, object userState) {
            if ((onBeginSendEmailDelegate == null)) {
                onBeginSendEmailDelegate = new BeginOperationDelegate(OnBeginSendEmail);
            }
            if ((onEndSendEmailDelegate == null)) {
                onEndSendEmailDelegate = new EndOperationDelegate(OnEndSendEmail);
            }
            if ((onSendEmailCompletedDelegate == null)) {
                onSendEmailCompletedDelegate = new System.Threading.SendOrPostCallback(OnSendEmailCompleted);
            }
            base.InvokeAsync(onBeginSendEmailDelegate, new object[] {
                        UserName,
                        UserEmail,
                        Message,
                        imgAttachment}, onEndSendEmailDelegate, onSendEmailCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult HRMSWEBAP.MailReference.MailService.BeginSendQueryMail(string Username, string userEmail, string subject, string message, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginSendQueryMail(Username, userEmail, subject, message, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        bool HRMSWEBAP.MailReference.MailService.EndSendQueryMail(System.IAsyncResult result) {
            return base.Channel.EndSendQueryMail(result);
        }
        
        private System.IAsyncResult OnBeginSendQueryMail(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string Username = ((string)(inValues[0]));
            string userEmail = ((string)(inValues[1]));
            string subject = ((string)(inValues[2]));
            string message = ((string)(inValues[3]));
            return ((HRMSWEBAP.MailReference.MailService)(this)).BeginSendQueryMail(Username, userEmail, subject, message, callback, asyncState);
        }
        
        private object[] OnEndSendQueryMail(System.IAsyncResult result) {
            bool retVal = ((HRMSWEBAP.MailReference.MailService)(this)).EndSendQueryMail(result);
            return new object[] {
                    retVal};
        }
        
        private void OnSendQueryMailCompleted(object state) {
            if ((SendQueryMailCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                SendQueryMailCompleted(this, new SendQueryMailCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void SendQueryMailAsync(string Username, string userEmail, string subject, string message) {
            SendQueryMailAsync(Username, userEmail, subject, message, null);
        }
        
        public void SendQueryMailAsync(string Username, string userEmail, string subject, string message, object userState) {
            if ((onBeginSendQueryMailDelegate == null)) {
                onBeginSendQueryMailDelegate = new BeginOperationDelegate(OnBeginSendQueryMail);
            }
            if ((onEndSendQueryMailDelegate == null)) {
                onEndSendQueryMailDelegate = new EndOperationDelegate(OnEndSendQueryMail);
            }
            if ((onSendQueryMailCompletedDelegate == null)) {
                onSendQueryMailCompletedDelegate = new System.Threading.SendOrPostCallback(OnSendQueryMailCompleted);
            }
            base.InvokeAsync(onBeginSendQueryMailDelegate, new object[] {
                        Username,
                        userEmail,
                        subject,
                        message}, onEndSendQueryMailDelegate, onSendQueryMailCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginOpen(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(callback, asyncState);
        }
        
        private object[] OnEndOpen(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndOpen(result);
            return null;
        }
        
        private void OnOpenCompleted(object state) {
            if ((OpenCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                OpenCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void OpenAsync() {
            OpenAsync(null);
        }
        
        public void OpenAsync(object userState) {
            if ((onBeginOpenDelegate == null)) {
                onBeginOpenDelegate = new BeginOperationDelegate(OnBeginOpen);
            }
            if ((onEndOpenDelegate == null)) {
                onEndOpenDelegate = new EndOperationDelegate(OnEndOpen);
            }
            if ((onOpenCompletedDelegate == null)) {
                onOpenCompletedDelegate = new System.Threading.SendOrPostCallback(OnOpenCompleted);
            }
            base.InvokeAsync(onBeginOpenDelegate, null, onEndOpenDelegate, onOpenCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginClose(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginClose(callback, asyncState);
        }
        
        private object[] OnEndClose(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndClose(result);
            return null;
        }
        
        private void OnCloseCompleted(object state) {
            if ((CloseCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                CloseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CloseAsync() {
            CloseAsync(null);
        }
        
        public void CloseAsync(object userState) {
            if ((onBeginCloseDelegate == null)) {
                onBeginCloseDelegate = new BeginOperationDelegate(OnBeginClose);
            }
            if ((onEndCloseDelegate == null)) {
                onEndCloseDelegate = new EndOperationDelegate(OnEndClose);
            }
            if ((onCloseCompletedDelegate == null)) {
                onCloseCompletedDelegate = new System.Threading.SendOrPostCallback(OnCloseCompleted);
            }
            base.InvokeAsync(onBeginCloseDelegate, null, onEndCloseDelegate, onCloseCompletedDelegate, userState);
        }
        
        protected override HRMSWEBAP.MailReference.MailService CreateChannel() {
            return new MailServiceClientChannel(this);
        }
        
        private class MailServiceClientChannel : ChannelBase<HRMSWEBAP.MailReference.MailService>, HRMSWEBAP.MailReference.MailService {
            
            public MailServiceClientChannel(System.ServiceModel.ClientBase<HRMSWEBAP.MailReference.MailService> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginSendEmail(string UserName, string UserEmail, string Message, string imgAttachment, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[4];
                _args[0] = UserName;
                _args[1] = UserEmail;
                _args[2] = Message;
                _args[3] = imgAttachment;
                System.IAsyncResult _result = base.BeginInvoke("SendEmail", _args, callback, asyncState);
                return _result;
            }
            
            public bool EndSendEmail(System.IAsyncResult result) {
                object[] _args = new object[0];
                bool _result = ((bool)(base.EndInvoke("SendEmail", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginSendQueryMail(string Username, string userEmail, string subject, string message, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[4];
                _args[0] = Username;
                _args[1] = userEmail;
                _args[2] = subject;
                _args[3] = message;
                System.IAsyncResult _result = base.BeginInvoke("SendQueryMail", _args, callback, asyncState);
                return _result;
            }
            
            public bool EndSendQueryMail(System.IAsyncResult result) {
                object[] _args = new object[0];
                bool _result = ((bool)(base.EndInvoke("SendQueryMail", _args, result)));
                return _result;
            }
        }
    }
}