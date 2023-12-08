import axios, { AxiosResponse } from "axios";
import { IFile } from "./models/file";

/** Добавляет базовый адрес для всех запросов (.env.development .env.production) */
axios.defaults.baseURL = process.env.REACT_APP_API_URL;

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
    get: <T>(url: string) => axios.get<T>(url).then(responseBody),
    post: <T>(url: string, body?: any) => axios.post<T>(url, body).then(responseBody),
    postForm: <T>(url: string, form: FormData) => axios.post<T>(url, form).then(responseBody),
    postFormWithConfig: <T>(url: string, form: FormData, config: any) => axios.post<T>(url, form, config).then(responseBody),
    put: <T>(url: string, body?: any) => axios.put<T>(url, body).then(responseBody),
    del: <T>(url: string) => axios.delete<T>(url).then(responseBody),
}



const File = {
    getFiles: () => requests.get<IFile[]>('/file'),
    upload: (form: FormData) => requests.postForm('/file', form),
    uploadWithConfig: (form: FormData, config: any) => requests.postFormWithConfig('/file', form, config),
    deleteFile:  (fileName: string) => requests.del<boolean>(`/file/${fileName}`),
}


const agent = {   
    File
}

export default agent;