import React from "react";
import fileImg from './images/file.png';
import folderImg from './images/folder.png';
import { store } from "../../../stores/store";

export default function FileRecord(props: any) {

    const dt = toJavaScriptDate(props.ts);

    const statuses = ["Uploaded", "Converting", "Error", "Ready"];

    let imgType = fileImg;
    if(props.type === "folder")
        imgType = folderImg;

    let pdfLink;     
    if(props.status < 3)
    {
        pdfLink = "";
    }
    else
    {
        pdfLink = <a href={`${process.env.REACT_APP_API_URL}/file/${props.pdfName}`}>[Download as PDF]</a>
    } 
    
    return (     
        <tr>
            <td><img src={imgType} alt="type"/></td>
            <td><a href={`${process.env.REACT_APP_API_URL}/file/${props.name}`}>{props.name}</a></td>
            <td>{statuses[props.status]}</td>
            <td>{dt.toLocaleString("ru-RU")}</td>            
            <td>{pdfLink}</td>            
            <td><button title="Delete file" onClick={async () => {await deleteFile(props.name)}}>❌</button></td>            
        </tr>       
    )  
}

function toJavaScriptDate(value: string): Date {
	return new Date(Date.parse(value));
}


async function deleteFile(fileName:string) {
    try {
        const success = await store.fileStore.deleteFile(fileName);
        if(success)
            store.fileStore.reloadFilesCallback!();
         
    }
    catch(err) {
        console.error(`Error occured while getting files from server: ${err}`);
    }   
}