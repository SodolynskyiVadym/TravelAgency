import { Inject, Injectable, InjectionToken } from '@angular/core';


export const BROWSER_STORAGE = new InjectionToken<Storage>('Browser Storage', {
  providedIn: 'root',
  factory: () => localStorage,
});


@Injectable()
export class BrowserStorageService {
  constructor(@Inject(BROWSER_STORAGE) public storage: Storage) {}

  get(key: string) {
    console.log('Getting key', key);
    console.log('Value:', this.storage.getItem(key));
    return this.storage.getItem(key);
  }

  set(key: string, value: string) {
    console.log('Setting key', key, 'to value', value);
    this.storage.setItem(key, value);
  }

  remove(key: string) {
    this.storage.removeItem(key);
  }

  clear() {
    this.storage.clear();
  }
}