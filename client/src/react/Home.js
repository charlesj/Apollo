import React from "react";
import Summaries from "./meta/Summaries";
import Weather from "./weather/Weather";
import Strategies from "./thinking/ObliqueStrategies";

function Home(props) {
  return (
    <div>
      <Weather />
      <Summaries />
      <Strategies />
    </div>
  );
}

export default Home;
