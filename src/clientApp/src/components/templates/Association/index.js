import React, { Component, Fragment } from "react";
import AssociationItem from "../../organisms/Association/AssociationItem";
import { Pagination } from "../../molecules/Paging";
import Breadcrumb from "../../molecules/Breadcrumb";

export default class extends Component {
  render() {
    const {
      associations,
      breadcrumbs,
      totalPage,
      baseUrl,
      currentPage,
    } = this.props;

    return (
      <Fragment>
        <Breadcrumb list={breadcrumbs} />
        <div className="row">
          {associations
            ? associations.map((item, index) => (
                <div
                  key={index}
                  className="col col-12 col-sm-6 col-md-6 col-lg-6 col-xl-4"
                >
                  <AssociationItem key={item.id} association={item} />
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
