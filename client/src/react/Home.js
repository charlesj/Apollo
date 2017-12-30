import React from "react";
import Summaries from "./meta/Summaries";
import Weather from "./weather/Weather";
import Strategies from "./thinking/ObliqueStrategies";
import CompleteChecklist from "./checklists/CompleteChecklist";

function Home(props) {
  return (
    <div>
      <Weather />
      <Summaries />
      <CompleteChecklist />
      <Strategies />
    </div>
  );
}

export default Home;
