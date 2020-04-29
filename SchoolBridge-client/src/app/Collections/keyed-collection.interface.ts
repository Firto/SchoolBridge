export interface IKeyedCollection<K,T> {
    addOrUpdate(key: K, value: T);
    addOrUpdateShift(key: K, value: T);
    containsKey(key: K): boolean;
    length(): number;
    item(key: K): T;
    keys(): K[];
    remove(key: K): T;
    values(): T[];
    clear(): void;
}