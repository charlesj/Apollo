import React from "react";
import Summaries from "./meta/Summaries";
import Weather from "./weather/Weather";
import Strategies from "./thinking/ObliqueStrategies";
import CompleteChecklist from "./checklists/CompleteChecklist";
import LifeCounter from "./meta/LifeCounter";
import FeaturedGoal from "./goals/FeaturedGoal";
import Timeline from "./timeline/Timeline";

function Home(props) {
  const news = [
    { link: "https://www.axios.com", label: "Axios" },
    { link: "https://www.bloomberg.com/", label: "Bloomberg" },
    { link: "https://www.theatlantic.com/", label: "The Atlantic" },
    { link: "https://www.nytimes.com", label: "The New York Times" },
    { link: "https://www.wsj.com/", label: "The Wall Street Journal" },
    { link: "https://www.washingtonpost.com/", label: "The Washington Post" },
    { link: "https://www.seattletimes.com/", label: "Seattle Times" },
    { link: "https://www.foreignaffairs.com/", label: "Foreign Affairs" },
    { link: "https://www.reuters.com/", label: "Reuters Wire" }
  ];
  return (
    <div>
      <LifeCounter />
      <FeaturedGoal />
      <Weather />
      <Summaries />
      <CompleteChecklist />
      <Strategies />
      <Timeline />
      <div>
        <ul>
          {news.map(n => {
            return (
              <li key={n.label}>
                <a href={n.link}>{n.label}</a>
              </li>
            );
          })}
        </ul>
      </div>
    </div>
  );
}

export default Home;
