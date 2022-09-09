using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using TowerSoft.Repository.Attributes;

namespace TowerSoft.Repository.Maps {
    /// <summary>
    /// Class that process and stores the mapping info for the repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MappingModel<T> {
        /// <summary>
        /// Autonumber map, if there is one
        /// </summary>
        public IMap AutonumberMap { get; private set; }

        /// <summary>
        /// All maps
        /// </summary>
        public List<IMap> AllMaps { get; private set; }

        /// <summary>
        /// All non-primary key maps
        /// </summary>
        public List<IMap> StandardMaps { get; private set; }

        /// <summary>
        /// All primary key maps. Includes autonumber maps
        /// </summary>
        public List<IMap> PrimaryKeyMaps { get; private set; }

        /// <summary>
        /// Property name / Column name dictionary of all maps
        /// </summary>
        public Dictionary<string, string> AllMapsDictionary { get; set; }

        private bool IgnoreObjectMaps { get; set; }

        internal MappingModel(bool ignoreObjectMaps) {
            IgnoreObjectMaps = ignoreObjectMaps;
            InitializeLists();
            GetColumnMappingUsingReflection();
            FinalizeMapReading();
        }

        internal MappingModel(EntityMap<T> entityMap) {
            InitializeLists();
            GetColumnMappingFromEntityMap(entityMap);
            FinalizeMapReading();
        }

        internal MappingModel(IEnumerable<IMap> maps) {
            InitializeLists();
            GetColumnMappingFromList(maps);
            FinalizeMapReading();
        }

        private void InitializeLists() {
            AllMaps = new List<IMap>();
            StandardMaps = new List<IMap>();
            PrimaryKeyMaps = new List<IMap>();
            AllMapsDictionary = new Dictionary<string, string>();
        }

        #region Map Loaders
        private void GetColumnMappingUsingReflection() {
            Type domainType = typeof(T);

            foreach (PropertyInfo prop in domainType.GetProperties()) {
                if (prop.IsDefined(typeof(NotMappedAttribute))) continue;
                if (IgnoreObjectMaps && (prop.Name.EndsWith("_Object") || prop.Name.EndsWith("_Objects"))) continue;

                if (prop.GetMethod != null && prop.GetMethod.IsPublic &&
                prop.SetMethod != null && prop.SetMethod.IsPublic) {

                    // Column Name
                    string columnName = prop.Name;
                    string functionName = null;
                    if (prop.IsDefined(typeof(ColumnMapAttribute))) {
                        ColumnMapAttribute colMapAttribute = prop.GetCustomAttribute<ColumnMapAttribute>();
                        if (!string.IsNullOrWhiteSpace(colMapAttribute.ColumnName)) {
                            columnName = colMapAttribute.ColumnName;
                        }
                        if (!string.IsNullOrWhiteSpace(colMapAttribute.FunctionName)) {
                            functionName = colMapAttribute.FunctionName;
                        }
                    } else if (prop.IsDefined(typeof(ColumnAttribute))) {
                        ColumnAttribute colAttribute = (ColumnAttribute)prop.GetCustomAttribute(typeof(ColumnAttribute));
                        if (!string.IsNullOrWhiteSpace(colAttribute.Name)) {
                            columnName = colAttribute.Name;
                        }
                    }

                    if (prop.IsDefined(typeof(AutonumberAttribute), true)) {
                        // Autonumber Map
                        if (AutonumberMap != null)
                            throw new Exception("Entity " + domainType.Name + " cannot have more than one autonumber map.");
                        AutonumberMap = new AutonumberMap(prop.Name, columnName, functionName);
                        PrimaryKeyMaps.Add(AutonumberMap);

                    } else if (prop.IsDefined(typeof(KeyAttribute))) {
                        // ID Maps
                        IDMap idMap = new IDMap(prop.Name, columnName, functionName);
                        PrimaryKeyMaps.Add(idMap);
                    } else if (prop.IsDefined(typeof(CacheFilemanDateTimeAttribute))) {
                        // Cache Fileman DateTime
                        CacheFilemanDateTimeMap filemanDateTimeMap = new CacheFilemanDateTimeMap(prop.Name, columnName, functionName);
                        StandardMaps.Add(filemanDateTimeMap);
                    } else if (prop.IsDefined(typeof(CacheFilemanDateAttribute))) {
                        // Cache Fileman Date
                        CacheFilemanDateMap filemanDateMap = new CacheFilemanDateMap(prop.Name, columnName, functionName);
                        StandardMaps.Add(filemanDateMap);
                    } else if (prop.IsDefined(typeof(CacheHorologDateAttribute))) {
                        // Cache Horolog Date
                        CacheHorologDateMap horologDateMap = new CacheHorologDateMap(prop.Name, columnName, functionName);
                        StandardMaps.Add(horologDateMap);
                    } else {
                        // Standard Maps
                        StandardMaps.Add(new Map(prop.Name, columnName, functionName));
                    }
                }
            }
        }

        private void GetColumnMappingFromEntityMap(EntityMap<T> entityMap) {
            List<IMap> maps = entityMap.GetMaps().ToList();

            IEnumerable<Map> defaultMaps = null;
            if (entityMap.EntityMapOption == AutoMappingOption.UseDefaultPropertyMaps ||
                entityMap.EntityMapOption == AutoMappingOption.UseNonObjectPropertyMaps) {
                defaultMaps = GetDefaultMaps(entityMap.EntityMapOption);

                if (defaultMaps != null) {
                    foreach (Map defaultMap in defaultMaps) {
                        if (!maps.Any(x => x.PropertyName.Equals(defaultMap.PropertyName, StringComparison.InvariantCultureIgnoreCase))) {
                            maps.Add(defaultMap);
                        }
                    }
                }
            }
            ProcessMapList(maps);
        }

        private void GetColumnMappingFromList(IEnumerable<IMap> maps) {
            ProcessMapList(maps);
        }
        #endregion

        private void ProcessMapList(IEnumerable<IMap> maps) {
            foreach (IMap map in maps) {
                if (map.ColumnName == null) continue;

                if (map is AutonumberMap) {
                    if (AutonumberMap != null)
                        throw new Exception("Multiple autonumber maps cannot be assigned");
                    if (PrimaryKeyMaps.Any())
                        throw new Exception("The class already has ID maps assigned. Autonumbers cannot be used with IDMaps");
                    AutonumberMap = map as AutonumberMap;
                    PrimaryKeyMaps.Add(map as Map);
                } else if (map is IDMap) {
                    if (AutonumberMap != null)
                        throw new Exception("Autonumber map already assigned entities cannot have autonumber and ID maps");
                    PrimaryKeyMaps.Add(map as Map);
                } else {
                    StandardMaps.Add(map as Map);
                }
            }
        }

        private void FinalizeMapReading() {
            if (!PrimaryKeyMaps.Any())
                throw new Exception("No primary key maps found on " + typeof(T).Name +
                    ". Use the AutonumberAttribute or KeyAttribute to mark a property as a primary key.");

            if (StandardMaps.Any())
                AllMaps.AddRange(StandardMaps);
            AllMaps.AddRange(PrimaryKeyMaps);

            if (!AllMaps.Any())
                throw new Exception("No mappable properties found on " + typeof(T).Name);

            AllMapsDictionary = new Dictionary<string, string>();
            foreach (IMap map in AllMaps) {
                AllMapsDictionary.Add(map.PropertyName, map.ColumnName);
            }
        }

        private IEnumerable<Map> GetDefaultMaps(AutoMappingOption option) {
            List<Map> maps = new List<Map>();
            foreach (PropertyInfo prop in typeof(T).GetProperties()) {
                if (prop.IsDefined(typeof(NotMappedAttribute))) continue;
                if (option == AutoMappingOption.UseNonObjectPropertyMaps && (prop.Name.EndsWith("_Object") || prop.Name.EndsWith("_Objects"))) continue;
                if (prop.GetMethod != null && prop.GetMethod.IsPublic &&
                    prop.SetMethod != null && prop.SetMethod.IsPublic) {
                    maps.Add(new Map(prop.Name));
                }
            }
            return maps;
        }
    }
}
