using System;
using SomeDependencies;

namespace ConsoleApp1
{
    internal class Startup : IStartup
    {
        public void Start()
        {
            var model = new Model {Identifier = 1, SomeText = "Some Text"};

            var viewModel = AutoMapper.Mapper.Map<ViewModel>(model);

            Console.WriteLine($"Viewmodel's Id = `{viewModel.Id}` and the Name is `{viewModel.Name}`.");
        }
    }
}
