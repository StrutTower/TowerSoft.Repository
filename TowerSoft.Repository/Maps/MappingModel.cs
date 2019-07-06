using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using TowerSoft.Repository.Attributes;

namespace TowerSoft.Repository.Maps {
    public class MappingModel<T> {
        /// <summary>
        /// Name of the database table
        /// </summary>
        public  string TableName { get; private set; }

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
        public List<JoinedMap> JoinedMaps { get; private set; }

        /// <summary>
        /// Property name / Column name dictionary of all maps
        /// </summary>
        public Dictionary<string, string> AllMapsDictionary { get; set; }


        public MappingModel() {
            InitializeLists();
            GetTableColumnMapping();
            FinalizeMapReading();
        }

        public MappingModel(EntityMap<T> entityMap) {
            InitializeLists();
            GetTableColumnMappingFromEntityMap(entityMap);
            FinalizeMapReading();
        }

        private void InitializeLists() {
            AllMaps = new List<IMap>();
            StandardMaps = new List<IMap>();
            PrimaryKeyMaps = new List<IMap>();
            JoinedMaps = new List<JoinedMap>();
            AllMapsDictionary = new Dictionary<string, string>();
        }

        private void GetTableColumnMapping() {
            Type domainType = typeof(T);

            TableName = domainType.Name;
            if (domainType.IsDefined(typeof(TableAttribute))) {
                TableAttribute tableAttr = (TableAttribute)domainType.GetCustomAttribute(typeof(TableAttribute));
                if (!string.IsNullOrWhiteSpace(tableAttr.Name)) {
                    TableName = tableAttr.Name;
                }
            }

            foreach (PropertyInfo prop in domainType.GetProperties()) {
                if (prop.IsDefined(typeof(NotMappedAttribute))) continue;

                if (prop.GetMethod != null && prop.GetMethod.IsPublic &&
                    prop.SetMethod != null && prop.SetMethod.IsPublic &&
                    !prop.GetMethod.IsVirtual) {

                    // Column Name
                    string columnName = prop.Name;
                    if (prop.IsDefined(typeof(ColumnAttribute))) {
                        ColumnAttribute colAttribute = (ColumnAttribute)prop.GetCustomAttribute(typeof(ColumnAttribute));
                        if (!string.IsNullOrWhiteSpace(colAttribute.Name)) {
                            columnName = colAttribute.Name;
                        }
                    }

                    if (prop.IsDefined(typeof(AutonumberAttribute))) {
                        // Autonumber Map
                        if (AutonumberMap != null)
                            throw new Exception("Entity " + domainType.Name + " cannot have more than one autonumber map.");
                        AutonumberMap = new AutonumberMap(prop.Name, columnName);
                        PrimaryKeyMaps.Add(AutonumberMap);

                    } else if (prop.IsDefined(typeof(KeyAttribute))) {
                        // ID Maps
                        IDMap idMap = new IDMap(prop.Name, columnName);
                        PrimaryKeyMaps.Add(idMap);

                    } else if (prop.IsDefined(typeof(JoinedMapAttribute))) {
                        // Joined Maps
                        JoinedMapAttribute attr = (JoinedMapAttribute)prop.GetCustomAttribute(typeof(JoinedMapAttribute));
                        JoinedMap joinedMap = new JoinedMap(prop.Name, attr.TableAndColumnName, attr.JoinStatement);
                        JoinedMaps.Add(joinedMap);
                    } else {
                        // Standard Maps
                        StandardMaps.Add(new Map(prop.Name, columnName));
                    }
                }
            }
        }

        private void GetTableColumnMappingFromEntityMap(EntityMap<T> entityMap) {
            TableName = entityMap.TableName;
            List<IMap> maps = entityMap.GetMaps().ToList();

            IEnumerable<Map> defaultMaps = null;
            if (entityMap.EntityMapOption == AutoMappingOption.UseDefaultPropertyMaps) {
                defaultMaps = GetDefaultMaps();

                if (defaultMaps != null) {
                    foreach (Map defaultMap in defaultMaps) {
                        if (!maps.Any(x => x.PropertyName.Equals(defaultMap.PropertyName, StringComparison.InvariantCultureIgnoreCase))) {
                            maps.Add(defaultMap);
                        }
                    }
                }
            }

            foreach (IMap map in maps) {
                if (map.ColumnName == null) continue;

                if (map is AutonumberMap) {
                    if (AutonumberMap != null)
                        throw new Exception("Multiple autonumber maps cannot be assigned");
                    if (PrimaryKeyMaps.Any())
                        throw new Exception("The class already has ID maps assigned. Autonumbers cannot be used eith IDMaps");
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

        private IEnumerable<Map> GetDefaultMaps() {
            List<Map> maps = new List<Map>();
            foreach (PropertyInfo prop in typeof(T).GetProperties()) {
                if (prop.GetMethod != null && prop.GetMethod.IsPublic &&
                    prop.SetMethod != null && prop.SetMethod.IsPublic &&
                    !prop.GetMethod.IsVirtual) {
                    maps.Add(new Map(prop.Name));
                }
            }
            return maps;
        }
    }
}
