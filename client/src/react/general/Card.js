import React from 'react';
import "./Card.css";

function Card(props){
  const { title, content } = props;
  return (<div className="card">
    <div className="cardContent">{content}</div>
    <div className="cardTitle">{title}</div>
  </div>)
}

export default Card;
