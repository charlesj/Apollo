import React from "react";
import Summaries from "./meta/Summaries";
import Weather from "./weather/Weather";
import Strategies from "./thinking/ObliqueStrategies";
import CompleteChecklist from "./checklists/CompleteChecklist";
import LifeCounter from "./meta/LifeCounter";
import FeaturedGoal from "./goals/FeaturedGoal";

function Home(props) {
  return (
    <div>
      <LifeCounter />
      <FeaturedGoal />
      <Weather />
      <Summaries />
      <CompleteChecklist />
      <Strategies />
    </div>
  );
}

export default Home;
