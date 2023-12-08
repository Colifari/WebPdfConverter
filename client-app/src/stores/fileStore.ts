import agent from "../agent";
import { IFile } from "../models/file";

/** Хранилище данных файлов */
export default class FileStore {

    async getFiles(): Promise<IFile[]> {
        return await agent.File.getFiles();
    }

    async deleteFile(fileName:string): Promise<boolean> {
        return await agent.File.deleteFile(fileName);
    }

    reloadFilesCallback: (() => void) | undefined;
}