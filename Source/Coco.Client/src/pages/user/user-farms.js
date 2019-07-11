import React, { Component, Fragment } from "react";
import { withRouter } from "react-router-dom";
import { UrlConstant } from "../../utils/Constant";
import { Pagination } from "../../components/molecules/Paging";
import FarmItem from "../../components/organisms/Farm/FarmItem";

export default withRouter(
  class extends Component {
    constructor(props) {
      super(props);

      let farms = [];
      for (let i = 0; i < 9; i++) {
        const farmItem = {
          id: i + 1,
          creator: {
            photoUrl: `${process.env.PUBLIC_URL}/photos/farmer-avatar.jpg`,
            profileUrl: "/profile/4976920d11d17ddb37cd40c54330ba8e",
            name: "Ông 5 Đất"
          },
          thumbnailUrl: `${process.env.PUBLIC_URL}/photos/farm1.jpg`,
          description:
            "Trang trại nằm ở gần cầu Hàm Luông, có nuôi và trồng khá nhiều cây trồng vật nuôi, có cả homestay để nghĩ ngơi với những nhà sàn bên sông rất mát",
          url: `${UrlConstant.Farm.url}1`,
          commentNumber: "14",
          reactionNumber: "45+",
          name: "Trang trại ông Năm Đất",
          contentType: 3,
          address:
            "123 Lò Sơn, ấp Gì Đó, xã Không Biết, huyện Cần Đước, Long An"
        };

        farms.push(farmItem);
      }

      const { location, pageNumber } = this.props;

      this.state = {
        farms,
        totalPage: 10,
        pageQuery: location.search,
        baseUrl: this.props.userUrl + "/farms",
        currentPage: pageNumber ? pageNumber : 1
      };
    }

    render() {
      const { farms, totalPage, baseUrl, currentPage, pageQuery } = this.state;
      return (
        <Fragment>
          <div className="row">
            {farms
              ? farms.map((item, index) => (
                  <div
                    key={index}
                    className="col col-12 col-sm-6 col-md-6 col-lg-6 col-xl-4"
                  >
                    <FarmItem key={item.id} farm={item} />
                  </div>
                ))
              : null}
          </div>
          <Pagination
            totalPage={totalPage}
            baseUrl={baseUrl}
            pageQuery={pageQuery}
            currentPage={currentPage}
          />
        </Fragment>
      );
    }
  }
);
