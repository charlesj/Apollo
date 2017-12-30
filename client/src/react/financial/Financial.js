import React, { Component } from "react";
import moment from "moment";
import apolloServer from "../../services/apolloServer";

class Finance extends Component {
  constructor(props) {
    super(props);
    this.state = {
      assetPrices: []
    };

    this.loadAssetPrices = this.loadAssetPrices.bind(this);
  }

  componentDidMount() {
    this.loadAssetPrices();
  }

  loadAssetPrices() {
    var assets = ["btc", "eth", "ltc"];
    assets.forEach(a => {
      apolloServer
        .invoke("getAssetPrice", {
          symbol: a
        })
        .then(price => {
          if (!price) {
            return;
          }
          var currentPrices = this.state.assetPrices;
          currentPrices.push(price);
          currentPrices.sort((a, b) => {
            return a.symbol > b.symbol;
          });
          this.setState({
            assetPrices: currentPrices
          });
        });
    });
  }

  render() {
    return (
      <div className="summaryContainer">
        Financial
        {this.state.assetPrices.map(ap => {
          var validAt = moment(ap.valid_at);
          return (
            <div key={ap.id} className="summary">
              <div className="summaryAmount">${ap.price}</div>
              <div className="summaryLabel">
                {ap.symbol.toUpperCase()} ({validAt.calendar()})
              </div>
            </div>
          );
        })}
      </div>
    );
  }
}

export default Finance;
