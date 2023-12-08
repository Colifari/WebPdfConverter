import React, { Fragment } from "react";
import FilesPanel from "./component/FilesPanel";
import UploadPanel from "./component/UploadPanel";
import "./component/fileManager.css";

export default function HomePage() {

    return (
        <Fragment>
            <FilesPanel />
            <UploadPanel />
        </Fragment>
    );
}