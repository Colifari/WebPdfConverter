import { createContext, useContext } from "react";
import FileStore from "./fileStore";

export interface IStore {
   
    /** Хранилище данных файлов */
    fileStore: FileStore;
}

export const store: IStore = {    
    fileStore: new FileStore()
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}