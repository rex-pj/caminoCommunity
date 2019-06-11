import React, { Fragment } from "react";
import loadable from "@loadable/component";
import FarmGroupItem from "../../organisms/FarmGroup/FarmGroupItem";
import { Pagination } from "../../molecules/Paging";
const Breadcrumb = loadable(() => import("../../molecules/Breadcrumb"));

export default function(props) {
  const { farmGroups, breadcrumbs, totalPage, baseUrl, currentPage } = props;

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
