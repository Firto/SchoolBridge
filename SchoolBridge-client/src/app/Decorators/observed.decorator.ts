import { Observable, Subscription } from 'rxjs';
import { Type, ɵComponentType as ComponentType } from '@angular/core';
import { markDirty } from '../Helpers/mark-dirty.func';

interface ComponentDefinition {
  onInit(): void;
  onDestroy(): void;
}

const noop = () => {
};

const getCmp = <T>(type: Function) => (type as any).ɵcmp as ComponentDefinition;
const subscriptionsSymbol = Symbol('__ng__subscriptions');

export function observed() {
  return function(
    target: object,
    propertyKey: string,
    descriptor?: PropertyDescriptor
  ) {
    if (descriptor) {
      const original = descriptor.value;
      descriptor.value = function(...args: any[]) {
        original.apply(this, args);
        markDirty(this);
      };
    } else {
      const cmp = getCmp(target.constructor);

      if (!cmp) {
        throw new Error(`Property ɵcmp is undefined`);
      }

      const onInit = cmp.onInit || noop;
      const onDestroy = cmp.onDestroy || noop;

      const getSubscriptions = (ctx) => {
        if (ctx[subscriptionsSymbol]) {
          return ctx[subscriptionsSymbol];
        }

        ctx[subscriptionsSymbol] = new Subscription();
        return ctx[subscriptionsSymbol];
      };

      const checkProperty = function(name: string) {
        const ctx = this;

        if (ctx[name] instanceof Observable) {
          const subscriptions = getSubscriptions(ctx);
          subscriptions.add(ctx[name].subscribe(() => markDirty(ctx)));
        } else {
          const handler = {
            set(obj: object, prop: string, value: unknown) {
              obj[prop] = value;
              markDirty(ctx);
              return true;
            }
          };

          ctx[name] = new Proxy(ctx, handler);
        }
      };

      const checkComponentProperties = (ctx) => {
        const props = Object.getOwnPropertyNames(ctx);

        props.map((prop) => {
          return Reflect.get(target, prop);
        }).filter(Boolean).forEach(() => {
          checkProperty.call(ctx, propertyKey);
        });
      };

      cmp.onInit = function() {
        const ctx = this;

        onInit.call(ctx);
        checkComponentProperties(ctx);
      };

      cmp.onDestroy = function() {
        const ctx = this;

        onDestroy.call(ctx);

        if (ctx[subscriptionsSymbol]) {
          ctx[subscriptionsSymbol].unsubscribe();
        }
      };

      Reflect.set(target, propertyKey, true);
    }
  };
}