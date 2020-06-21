import { IKeyedCollection } from './keyed-collection.interface';

export class KeyedCollection<K, T> implements IKeyedCollection<K, T> {
    public items: Array<{key:K, value: T}> = new Array<{key:K, value: T}>();
 
    public containsKey(key: K): boolean {
        return this.items.findIndex((x) => x.key == key) != -1;
    }

    public getIndex(key: K): number {
        return this.items.findIndex((x) => x.key == key);
    }
 
    public length(): number {
        return this.items.length;
    }
 
    public addOrUpdate(key: K, value: T) {
        const ind: number = this.getIndex(key);
        if(ind != -1)
            this.items[ind].value = value;
        else this.items.push({key:key, value: value});
    }

    public addOrUpdateShift(key: K, value: T) {
        const ind: number = this.getIndex(key);
        if(ind != -1)
            this.items[ind].value = value;
        else this.items.unshift({key:key, value: value});
    }
 
    public remove(key: K): T {
        const ind: number = this.getIndex(key);
        const val: T = this.items[ind].value;
        this.items.splice(ind, 1);
        return val;
    }
 
    public item(key: K): T {
        return this.items[this.getIndex(key)].value;
    }
 
    public keys(): K[] {
        const keySet: K[] = [];
        this.items.forEach((x) => keySet.push(x.key));
        return keySet;
    }
 
    public values(): T[] {
        const values: T[] = [];
        this.items.forEach((x) => values.push(x.value));
        return values;
    }

    public clear(): void{
        this.items.splice(0, this.items.length);
    }
}