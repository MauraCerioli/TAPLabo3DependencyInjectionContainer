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
                    if (!interfaceType.IsAssignableFrom(implementationType)){
                        throw new ArgumentException($"Type {implementationType.Name} does not implement {interfaceType.Name}");
                    }
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
                    type = assembly.GetType(typeName,true);
                }
                catch (Exception e) {
                    Debug.WriteLine(e);
                    throw new ArgumentException($"{assemblyFile} does not contain accessible type {typeName}", e);
                }
            }
        }
        public T Instantiate<T>() where T : class{
            Type implType;
            //TODO Change to accept also classes. If T is class implType=typeof(T)
            try{
                implType = KnownTypes[typeof(T)];
            }
            catch (Exception e){
                throw new ArgumentException($"Unknown type {typeof(T).Name}",e);
            }
            //TODO Change to use constructors with parameters
            /*
             * iteration on all implType constructors
             * foreach of them see if we can use it
             * - foreach parameter we can build it by a recursive call
             * - if we can build all parameters use them and return result of invocation
             * - otherwise pass to next
             * - if all fail throw
             * Warning: to avoid infinite loop in recursion
             * - use auxiliary method with extra parameter list of type with building in progress
             * - if a parameter is of a type in the list abort
             */
            return (T)Activator.CreateInstance(implType);
        }
    }
}
