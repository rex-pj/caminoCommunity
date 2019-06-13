import React, { Component, Fragment } from "react";
import { withRouter } from "react-router-dom";
import { UrlConstant } from "../../utils/Constant";
import { Pagination } from "../../components/molecules/Paging";
import ArticleListItem from "../../components/organisms/Article/ArticleListItem";

export default withRouter(
  class extends Component {
    constructor(props) {
      super(props);

      let articles = [];
      for (let i = 0; i < 8; i++) {
        const articleItem = {
          id: i + 1,
          creator: {
            photoUrl: `${process.env.PUBLIC_URL}/photos/farmer-avatar.jpg`,
            profileUrl: "/profile?id=SXaSDRHRfds3zUDFQzC6jg==",
            name: "Anh Sáu"
          },
          createdDate: "26/11/2018 9:28",
          updatedDate: "26/11/2018 9:28",
          thumbnailUrl: `${process.env.PUBLIC_URL}/photos/farmstay.jpg`,
          description:
            "Bọ rùa (Coccinellidae), hay còn gọi là bọ hoàng hậu, bọ cánh cam là tên gọi chung cho các loài côn trùng nhỏ, mình tròn hình cái trống, phủ giáp trụ, trên mặt cánh có những chấm đen (có loài không có). Người ta phân loại bọ rùa tùy theo số chấm và hình thái cơ thể Loài bọ rùa thường thấy nhất là bọ rùa 7 sao. Trên bộ cánh vỏ vàng cam có 7 nốt đen (mỗi cánh có ba nốt, còn một nốt ở chỗ giáp lại giữ hai cánh). Đây là loài bọ rùa to nhất và là một thợ săn đáng",
          url: `${UrlConstant.Article.url}1`,
          reactionNumber: "2.5k+",
          commentNumber: "14",
          name:
            "Ban quản lý một siêu thị lớn tại Mỹ thả khoảng 72.000 con bọ rùa vào các gian hàng để chúng diệt rệp vừng.",
          contentType: 1
        };

        articles.push(articleItem);
      }

      const { match, location } = this.props;
      const { params } = match;
      const { page } = params;

      this.state = {
        articles,
        totalPage: 10,
        pageQuery: location.search,
        baseUrl: this.props.userUrl + "/posts",
        currentPage: page ? page : 1
      };
    }

    render() {
      const {
        articles,
        totalPage,
        baseUrl,
        currentPage,
        pageQuery
      } = this.state;

      return (
        <Fragment>
          {articles
            ? articles.map(item => (
                <ArticleListItem key={item.id} article={item} />
              ))
            : null}
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
