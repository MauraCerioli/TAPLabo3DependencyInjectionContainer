using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TinyDependencyInjectionContainer {
    public class InterfaceResolver {
        private Dictionary<Type, Type> KnownTypes { get; }

        public InterfaceResolver(string configFileName) {
            string[] configLines;
            try{
                configLines = File.ReadAllLines(configFileName);
            }
            catch (Exception e){
                throw new ArgumentException($"Impossible to read file {configFileName}",e);
            }
            KnownTypes=new Dictionary<Type, Type>();
            foreach (var line in configLines) {
                if (line.Length != 0 && '#' != line[0]) {
                    var parts = line.Split('*');
                    if (4 != parts.Length)
                        throw new ArgumentException($"Config file with not weel-formed line ({line})");
                    GetTypeFromConfig(parts[0], parts[1], out var interfaceType);
                    GetTypeFromConfig(parts[2], parts[3], out var implementationType);
                    try{
                        KnownTypes.Add(interfaceType,implementationType);
                    }
                    catch (Exception e){
                        throw new ArgumentException($"Duplicated association for interface type {interfaceType.Name}",e);
                    }
                }
            }

            void GetTypeFromConfig(string assemblyFile, string typeName, out Type type) {
                Assembly assembly;
                try {
                    assembly = Assembly.LoadFrom(assemblyFile);
                }
                catch (Exception e) {
                    Debug.WriteLine(e);
                    throw new ArgumentException($"Impossible to load {assemblyFile}", e);
                }
                try {
                    type = assembly.GetType(typeName);
                }
                catch (Exception e) {
                    Debug.WriteLine(e);
                    throw new ArgumentException($"{assemblyFile} does not contain accessible type {typeName}", e);
                }
            }
        }
        public T Instantiate<T>() where T : class{
            Type implType;
            try{
                implType = KnownTypes[typeof(T)];
            }
            catch (Exception e){
                throw new ArgumentException($"Unknown type {typeof(T).Name}",e);
            }
            return (T)Activator.CreateInstance(implType);
        }
    }
}
