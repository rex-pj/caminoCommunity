import React, { Component, Fragment } from "react";
import FarmGroupItem from "../../organisms/FarmGroup/FarmGroupItem";
import { Pagination } from "../../molecules/Paging";
import Breadcrumb from "../../molecules/Breadcrumb";

export default class extends Component {
  render() {
    const {
      farmGroups,
      breadcrumbs,
      totalPage,
      baseUrl,
      currentPage
    } = this.props;

    return (
      <Fragment>
        <Breadcrumb list={breadcrumbs} />
        <div className="row">
          {farmGroups
            ? farmGroups.map((item, index) => (
                <div
                  key={index}
                  className="col col-12 col-sm-6 col-md-6 col-lg-6 col-xl-4"
                >
                  <FarmGroupItem key={item.id} farmGroup={item} />
                </div>
              ))
            : null}
        </div>
        <Pagination
          totalPage={totalPage}
          baseUrl={baseUrl}
          currentPage={currentPage}
        />
      </Fragment>
    );
  }
}
