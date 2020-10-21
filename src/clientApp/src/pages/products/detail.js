import React, { Fragment } from "react";
import { UrlConstant } from "../../utils/Constants";
import Detail from "../../components/templates/Product/Detail";
import Breadcrumb from "../../components/organisms/Navigation/Breadcrumb";
import { useQuery } from "@apollo/client";
import { GET_PRODUCT } from "../../utils/GraphQLQueries/queries";
import { withRouter } from "react-router-dom";

export default withRouter(function (props) {
  const { match } = props;
  const { params } = match;
  const { id } = params;

  const fetchRelationProducts = () => {
    let relationProducts = [];
    for (let i = 0; i < 6; i++) {
      const productItem = {
        id: i + 1,
        creator: {
          photoUrl: `${process.env.PUBLIC_URL}/photos/farmer-avatar.jpg`,
          profileUrl: "/profile/4976920d11d17ddb37cd40c54330ba8e",
          name: "Ông 5 Đất",
        },
        thumbnailUrl: `${process.env.PUBLIC_URL}/photos/banana.jpg`,
        farmUrl: `${UrlConstant.Farm.url}1`,
        farmName: "Trang trại ông năm đất",
        url: `${UrlConstant.Product.url}1`,
        commentNumber: "14",
        reactionNumber: "45+",
        name: "Chuối chính cây Đồng Nai",
        contentType: 2,
        price: 100000,
      };

      relationProducts.push(productItem);
    }
  };

  const { loading, data } = useQuery(GET_PRODUCT, {
    variables: {
      criterias: {
        id: parseFloat(id),
      },
    },
  });

  if (loading || !data) {
    return <Fragment></Fragment>;
  }

  const { product: productResponse } = data;
  const relationProducts = [];
  let product = { ...productResponse };

  const breadcrumbs = [
    {
      title: "Products",
      url: "/products/",
    },
    {
      isActived: true,
      title: product.name,
    },
  ];

  product.images = product.thumbnails.map((item) => {
    let image = { ...item };

    if (image.id > 0) {
      image.thumbnailUrl = `${process.env.REACT_APP_CDN_PHOTO_URL}${image.id}`;
      image.url = `${process.env.REACT_APP_CDN_PHOTO_URL}${image.id}`;
    }
    return image;
  });

  if (product.productFarms) {
    product.productFarms = product.productFarms.map((pf) => {
      let productFarm = { ...pf };
      productFarm.url = `/farms/${pf.farmId}`;
      return productFarm;
    });
  }

  return (
    <Fragment>
      <Breadcrumb list={breadcrumbs} />
      <Detail
        product={product}
        breadcrumbs={breadcrumbs}
        fetchRelationProducts={fetchRelationProducts}
        relationProducts={relationProducts}
      />
    </Fragment>
  );
});
