import React from "react";
import { IFile } from "../../../models/file";
import { store } from "../../../stores/store";
import FileRecord from "./FileRecord";

class FilesPanel extends React.Component {

    state: FilesPanelState;

    constructor(props: any) {
        super(props);
        this.state = new FilesPanelState();

        store.fileStore.reloadFilesCallback = async () => { await this.loadFiles(); }

        setInterval(async () => {await this.loadFiles()}, 15_000);
    }
    
    async componentDidMount() {
        await this.loadFiles();           
    }

    render() {

        const files = [];

        if(this.state)
        {
            for(let i=0;i<this.state.files.length;i++)
            {
                files.push(<FileRecord 
                    key={this.state.files[i].Name} 
                    name={this.state.files[i].Name} 
                    status={this.state.files[i].Status} 
                    ts={this.state.files[i].Timestamp}
                    pdfName={this.state.files[i].PdfName} 
                    />);
            }
        }

        return (
            <table className="filesTable">
                <thead>
                    <tr>
                        <td></td>
                        <td>Name</td>
                        <td>Status</td>
                        <td>Timestamp</td>
                        <td>PDF</td>
                        <td>Delete</td>
                    </tr>
                </thead>
                <tbody>
                    {files}
                </tbody>
            </table>                
        )
    }

    async loadFiles() {
        try {
            const files = await store.fileStore.getFiles();
            this.setState({...this.state.files, files: files}); 
        }
        catch(err) {
            console.error(`Error occured while getting files from server: ${err}`);
        }   
    }

}

class FilesPanelState {
    
    files: IFile[] = [];

}

export default FilesPanel;