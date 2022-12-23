using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpLine.Apps.Admin.API.Configuration.Correctors
{
    public class Corrector<T>
    {
        private IEnumerable<IDataCorrector<T>> _correctors;

        public Corrector(IEnumerable<IDataCorrector<T>> correctors)
        {
            _correctors = correctors;
        }

        public async Task Correct(T data)
        {
            foreach (var corrector in _correctors)
                await corrector.Correct(data);
        }

    }
}
