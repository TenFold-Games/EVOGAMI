using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting.Dependencies.NCalc;

namespace EVOGAMI.Utils
{
    public class SequenceMatcher
    {
        private readonly int _sequenceLength;
        
        private readonly HashSet<string> _recipes = new();
        private readonly StringBuilder _buffer = new();
        
        public delegate void SequenceCallback(string sequence);
        public event SequenceCallback OnSequenceRead = delegate {  };
        public event SequenceCallback OnSequenceBreak = delegate {  };
        public event SequenceCallback OnSequenceComplete = delegate {  };
        
        public SequenceMatcher(int sequenceLength)
        {
            _sequenceLength = sequenceLength;
        }
        
        public void AddRecipe(string recipe)
        {
            _recipes.Add(recipe);
        }
        
        public void ClearBuffer()
        {
            _buffer.Clear();
        }
        
        public string GetBuffer()
        {
            return _buffer.ToString();
        }

        public void OnSequencePerformed(char c)
        {
            _buffer.Append(c);
            
            // Trim the buffer if it exceeds the sequence length.
            if (_buffer.Length > _sequenceLength) _buffer.Remove(0, 1);
            
            OnSequenceRead(_buffer.ToString());

            if (_buffer.Length != _sequenceLength) // Buffer not full
            {
                if (!_recipes.Any(recipe => recipe.StartsWith(_buffer.ToString()))) 
                    OnSequenceBreak(_buffer.ToString());
            }
            else
            {
                // Buffer is full, check if it matches any of the recipes.
                if (_recipes.Contains(_buffer.ToString()))
                    OnSequenceComplete(_buffer.ToString());
                else
                    OnSequenceBreak(_buffer.ToString());
            }
        }
    }
}