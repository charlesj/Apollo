import React from 'react';
import { NavLink } from 'react-router-dom';
import FontAwesome from 'react-fontawesome';

function Nav() {
  return (
    <nav>
      <NavLink exact activeClassName='active' to='/'><FontAwesome name='home' /> Home</NavLink>
      <NavLink activeClassName='active' to='/organize'><FontAwesome name='check-square-o' /> Organize</NavLink>
      <NavLink activeClassName='active' to='/journal'><FontAwesome name='pencil' /> Log</NavLink>
      <NavLink activeClassName='active' to='/health'><FontAwesome name='medkit' /> Health</NavLink>
      <NavLink activeClassName='active' to='/bookmarks'><FontAwesome name='bookmark-o' /> Bookmarks</NavLink>
      <NavLink activeClassName='active' to='/jobs'><FontAwesome name='gavel' /> Jobs</NavLink>
      <NavLink activeClassName='active' to='/settings'><FontAwesome name='gear' /> Settings</NavLink>
    </nav>
  )
}

module.exports = Nav;
