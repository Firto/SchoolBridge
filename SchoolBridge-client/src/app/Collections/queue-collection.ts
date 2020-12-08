class Node<T> {
    next: Node<T>;
    constructor(public data: T) {
    }
  }
  
  export class Queue<T> {
    head: Node<T>;
    tail: Node<T>;
    length: number = 0;

    constructor() {
      this.head = this.tail = null;
    }
  
    enqueue(data: T): void {
        this.length++;
      const node = new Node(data);
  
      if (this.isEmpty()) {
        this.head = this.tail = node;
        return;
      }
      
      this.tail.next = node;
      this.tail = node;
    }
  
    dequeue(): T {
      if (this.isEmpty()) {
        return null;
      }
  
      const data = this.head.data;
  
      if (this.tail === this.head) {
        this.head = this.tail = null;
      } else {
        this.head =  this.head.next;
      }
      this.length--;
      return data;
    }
  
    isEmpty() {
      return this.head === null;
    }

    count(): number{
        return this.length;
    }
  }