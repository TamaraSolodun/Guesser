export class VaultService {
  storage = localStorage;

  setItem<T>(key: string, value: T): void {
    try {
      return this.storage.setItem(key, JSON.stringify(value));
    } catch (e) {
      console.warn('e: ', e);
    }
  }

  removeItem(key: string): void {
    this.storage.removeItem(key);
  }

  getItem<T = string>(key: string): T {
    try {
      return JSON.parse(this.storage.getItem(key) as string);
    } catch {}

    return (this.storage.getItem(key) as unknown) as T;
  }

  clearStorage(): void {
    this.storage.clear();
  }
}
