// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System.Collections.Generic;
using SharpCore.Logging;

namespace SharpDoc.Model
{
    /// <summary>
    /// A class that associate a xml id to a <see cref="IModelReference"/>.
    /// </summary>
    public class MemberRegistry
    {
        private readonly Dictionary<string, Dictionary<string, IModelReference>> _mapIdToModelElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRegistry"/> class.
        /// </summary>
        public MemberRegistry()
        {
            _mapIdToModelElement = new Dictionary<string, Dictionary<string, IModelReference>>();
        }


        /// <summary>
        /// Finds the by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public IModelReference FindById(string id)
        {
            return FindById(id, null);
        }

        /// <summary>
        /// Finds the by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="context">The context. If context is null, look inside all registered assemblies</param>
        /// <returns></returns>
        public IModelReference FindById(string id, IModelReference context)
        {
            IModelReference refFound = null;

            List<string> assemblyIds;

            if (context == null)
            {
                assemblyIds = new List<string>(_mapIdToModelElement.Keys);
            }
            else
            {
                assemblyIds = new List<string>();

                if (context is NMember)
                {
                    assemblyIds.Add(((NMember) (context)).Namespace.Assembly.Id);
                }
                else if (context is NNamespace)
                {
                    assemblyIds.Add(((NNamespace) (context)).Assembly.Id);
                }
                else if (context is NAssembly)
                {
                    assemblyIds.Add(((NAssembly)context).Id);
                }
            }

            foreach (var assemblyId in assemblyIds)
            {
                Dictionary<string, IModelReference> idToRef;
                if (_mapIdToModelElement.TryGetValue(assemblyId, out idToRef))
                {
                    if (idToRef.TryGetValue(id, out refFound))
                        return refFound;
                }
                
            }
            return refFound;
        }

        /// <summary>
        /// Registers the specified model element with the specified id.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="modelReference">The model element.</param>
        public void Register(NAssembly assembly, IModelReference modelReference)
        {
            Dictionary<string, IModelReference> idToRef;
            if (!_mapIdToModelElement.TryGetValue(assembly.Id, out idToRef))
            {
                idToRef = new Dictionary<string, IModelReference>();
                _mapIdToModelElement.Add(assembly.Id, idToRef);
            }

            if (idToRef.ContainsKey(modelReference.Id))
            {
                Logger.Warning("Id [{0}] already registered", modelReference.Id);
                return;
            }
            idToRef.Add(modelReference.Id, modelReference);
        }
    }
}