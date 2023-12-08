import React, { Fragment } from "react";
import agent from "../../../agent";
import { store } from "../../../stores/store";

const dialog = document.getElementsByTagName("dialog");
const ONE_KB = 1024;
const ONE_MB = 1048576;
const ONE_GB = 1073741824;


document.addEventListener("keydown", (ev) => { if(ev.key === "esc") dialog[0].close();  });


class UploadPanel extends React.Component {

    state: UploadPanelState;

    prevUpload = 0;
    prevUploadDt = new Date();
    uploaded: number = 0;
    total: number = 0;
    
    constructor(props: any) {
        super(props);
        this.state = new UploadPanelState();
    }
    
    async componentDidMount() {      
    }

    render() {

        return (
            <Fragment>
                <button className="uploadBtn" onClick={() => {this.uploadBtnClick()}}>Upload files</button>
                <dialog>
                    <div className="selectFile">
                        <form id="upload_form" onSubmit={(e) => {this.formSubmit(e)}}>
                            <input type="file" name="file" id="file" multiple accept=".htm, .html" />
                            <br/>  
                            <button type="submit">Upload</button>
                        </form>
                    </div>
                    <div className="uploadStatus">                    
                        <progress id="progressBar" value={this.state?.progressBar} max="100"></progress>
                        <p id="statusP">{this.bytesToString(this.state?.bytesLoaded)} / {this.bytesToString(this.state?.bytesTotal)}</p>
                        <p id="spdP">{this.bytesToString(this.state?.speed)} / s</p>
                        <p>Hit Esc to close popup</p>
                    </div>
                </dialog>       
            </Fragment>
            
        )
    }

    uploadBtnClick() {        
        dialog[0].showModal();        
    }

    async formSubmit(e: React.FormEvent) {
        e.preventDefault();

        this.prevUpload = 0;
        this.prevUploadDt = new Date();
        const input: HTMLInputElement | null = document.querySelector('input[type="file"]');

        let files: FileList;
        if(input && input.files)
            files = input.files;
        else
            return;
        
        const config = {
            onUploadProgress: function(this: UploadPanel, e: any) {
                if (e.event.lengthComputable) {
                    this.uploaded = e.loaded;
                    this.total = e.total;

                    let percent = 0;
                    if(e.progress)                    
                        percent  = e.progress * 100;                    
                    else
                        percent = Math.round((e.loaded / e.total) * 100);

                    let spd = 0;
                    if(e.rate)
                        spd = e.rate;
                    else
                        spd = this.calcSpd();

                    const newState = structuredClone(this.state);
                    if(percent === 100)
                    {                        
                        dialog[0].close(); 
                        store.fileStore.reloadFilesCallback!();
                    }                        

                    newState.progressBar = percent;
                    newState.bytesLoaded = e.loaded;        
                    newState.bytesTotal = e.total;  
                    newState.speed = spd;   
                    this.setState(newState);                   
            
                   
                }    
            }.bind(this),
            onload: function(this: UploadPanel, e: any) {

                const newState = structuredClone(this.state);
                newState.progressBar = 0;    // will clear progress bar after successful upload
                newState.speed = 0;    
                newState.eta = 0;             
                this.setState(newState);
                console.debug("onLoad()" + e);
            }.bind(this),
            onerror: function(e:any) {
                console.error("onerror() " + e);
            },
            onabort: function(e:any) {
                console.debug("onerror() " + e);
            },
            
        };

        for(let i=0; i<files.length;i++)
        {
            const formdata = new FormData();
            formdata.append("files", files[i], files[i].name);
    
            await agent.File.uploadWithConfig(formdata, config);
        }    
        
        //dialog[0].close();
        //store.fileStore.reloadFilesCallback!();  
    }

    // возвращает в байтах
    calcSpd(): number {        
        const now = new Date();
        const pass = now.getTime() - this.prevUploadDt.getTime();

        const speed: number = ((this.uploaded - this.prevUpload) * 1000) / pass;

        this.prevUpload = this.uploaded;
        this.prevUploadDt = now;
        return speed;        
    }

    bytesToString(bytes: number): string {
        if (bytes  <= ONE_KB) 
            return `${bytes} B`;
        
        if (bytes > ONE_KB && bytes <= ONE_MB) 
            return `${(bytes / 1024).toFixed(2)} Kb`;
        
        else if (bytes > ONE_MB && bytes <= ONE_GB)        
            return `${(bytes / ONE_MB).toFixed(2)} Mb`;
        
        return `${(bytes / ONE_GB).toFixed(2)} Gb`;
    }

    secondsToString(sec: number): string {
        if (sec < 60) {
            const s = Math.ceil(sec);
            if (s.toString().length === 1)
                return `00:0${s}`;
            else
                return `00:${s}`;
        }
        if (sec >= 60 && sec < 3600) {
            
            const m = Math.floor(sec / 60);
            let mstr = m.toString();
            if (mstr.length === 1)
                mstr = `0${m}`;
            
            const s = Math.floor(sec - (m * 60));
            let sstr = s.toString();
            if (sstr.length === 1)
                sstr = `0${s}`;
            return `${mstr}:${sstr}`;
        }
        else if (sec >= 3600 && sec <= 1073741824) {
            const h = Math.floor(sec / 3600);
            let hstr = h.toString();
            if (hstr.length === 1)
                hstr = `0${h}`;

            const m = Math.floor((sec - (h * 3600)) / 60);
            let mstr = m.toString();
            if (mstr.length === 1)
                mstr = `0${m}`;

            const s = Math.floor(sec - (h * 3600) - (m * 60));
            let sstr = s.toString();
            if(sstr.length === 1)
                sstr = `0${s}`;

            return `${hstr}:${mstr}:${sstr}`;
        }
        
        return "00:00:00";
    }

}

class UploadPanelState {    
    progressBar: number = 0;
    bytesLoaded: number = 0;
    bytesTotal: number = 0;
    eta: number = 0;
    speed: number = 0;
}

export default UploadPanel;