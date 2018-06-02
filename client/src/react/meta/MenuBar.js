import React, { Component } from "react";
import { Link } from 'react-router-dom';
import FontAwesome from "react-fontawesome";
import { FlexRow } from "../_controls";
import { MainRoutes } from '../../redux/navigator';
import ServerActivity from "./ServerActivity";
import ApplicationInfo from "./ApplicationInfo";
import "./MenuBar.css";

class MenuBar extends Component {
  render() {
    return (
      <FlexRow className="menuBar">
        <ServerActivity />
        { MainRoutes.map(r => {
          return <Link className='menuLink' to={r.path}>
            <FontAwesome name={r.icon} /> {r.label}
          </Link>
        })}
        <ApplicationInfo />
      </FlexRow>
    );
  }
}

export default MenuBar;
