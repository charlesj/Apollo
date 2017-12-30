import apolloServer from "./apolloServer";

export const addMetric = function(category, name, value) {
  return apolloServer.invoke("addMetric", {
    category: category,
    name: name,
    value: value
  });
};

export const getMetrics = function(category, name) {
  return apolloServer.invoke("getMetrics", {
    category: category,
    name: name
  });
};
