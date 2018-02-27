using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostSharp.Aspects;
using PostSharp.Serialization;
using Xamarin.Forms;

namespace App5
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var repo = new Repo();
            repo.SaveAsync();
        }
    }

    public class Repo
    {
        [RetryOnException]
        public void SaveAsync()
        {
            int a = 0;
        }
    }

    [PSerializable]
    public class RetryOnExceptionAttribute : MethodInterceptionAspect
    {
        public RetryOnExceptionAttribute()
        {
            this.MaxRetries = 3;
        }

        public int MaxRetries { get; set; }

        public override void OnInvoke(MethodInterceptionArgs args)
        {
            int retriesCounter = 0;

            args.Proceed();

            Debug.WriteLine(
                "Exception during attempt {0} of calling method {1}.{2}:", retriesCounter, args.Method.DeclaringType, args.Method.Name);

            return;
        }

        public override async Task OnInvokeAsync(MethodInterceptionArgs args)
        {
            int retriesCounter = 0;
            await args.ProceedAsync();

            Debug.WriteLine(
                "Exception during attempt {0} of calling method {1}.{2}:", retriesCounter, args.Method.DeclaringType, args.Method.Name);

            return;

            while (true)
            {
                try
                {

                }
                catch (Exception e)
                {
                    retriesCounter++;
                    if (retriesCounter > this.MaxRetries) throw;

                    Console.WriteLine(
                        "Exception during attempt {0} of calling method {1}.{2}: {3}",
                        retriesCounter, args.Method.DeclaringType, args.Method.Name, e.Message);
                }
            }
        }
    }
}
