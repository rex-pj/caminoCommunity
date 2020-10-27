import React, { Fragment } from "react";
import { UrlConstant } from "../../utils/Constants";
import Detail from "../../components/templates/Product/Detail";
import Breadcrumb from "../../components/organisms/Navigation/Breadcrumb";
import { useQuery } from "@apollo/client";
import styled from "styled-components";
import {
  GET_PRODUCT,
  GET_RELEVANT_PRODUCTS,
} from "../../utils/GraphQLQueries/queries";
import { withRouter } from "react-router-dom";
import ProductItem from "../../components/organisms/Product/ProductItem";
import { TertiaryHeading } from "../../components/atoms/Heading";
import Loading from "../../components/atoms/Loading";

const RelationBox = styled.div`
  margin-top: ${(p) => p.theme.size.distance};
`;

export default withRouter(function (props) {
  const { match } = props;
  const { params } = match;
  const { id } = params;

  const { loading, data } = useQuery(GET_PRODUCT, {
    variables: {
      criterias: {
        id: parseFloat(id),
      },
    },
  });

  const { relevantLoading, data: relevantData } = useQuery(
    GET_RELEVANT_PRODUCTS,
    {
      variables: {
        criterias: {
          id: parseFloat(id),
        },
      },
    }
  );

  if (loading || !data) {
    return <Loading>Loading...</Loading>;
  }

  const { product: productResponse } = data;
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

  if (product.thumbnails && product.thumbnails.length > 0) {
    product.images = product.thumbnails.map((item) => {
      let image = { ...item };

      if (image.id > 0) {
        image.thumbnailUrl = `${process.env.REACT_APP_CDN_PHOTO_URL}${image.id}`;
        image.url = `${process.env.REACT_APP_CDN_PHOTO_URL}${image.id}`;
      }
      return image;
    });
  }

  if (product.productFarms) {
    product.productFarms = product.productFarms.map((pf) => {
      let productFarm = { ...pf };
      productFarm.url = `/farms/${pf.farmId}`;
      return productFarm;
    });
  }

  const renderRelevants = (relevantLoading, relevantData) => {
    if (relevantLoading || !relevantData) {
      return <Loading className="mt-3">Loading...</Loading>;
    }
    const { relevantProducts } = relevantData;
    const relevants = relevantProducts.map((item) => {
      let productItem = { ...item };
      productItem.url = `${UrlConstant.Product.url}${productItem.id}`;
      if (productItem.thumbnails) {
        const thumbnail = productItem.thumbnails[0];
        if (thumbnail && thumbnail.id > 0) {
          productItem.thumbnailUrl = `${process.env.REACT_APP_CDN_PHOTO_URL}${thumbnail.id}`;
        }
      }

      productItem.creator = {
        createdDate: item.createdDate,
        profileUrl: `/profile/${item.createdByIdentityId}`,
        name: item.createdBy,
      };

      if (item.createdByPhotoCode) {
        productItem.creator.photoUrl = `${process.env.REACT_APP_CDN_AVATAR_API_URL}${item.createdByPhotoCode}`;
      }

      if (productItem.productFarms) {
        productItem.productFarms = productItem.productFarms.map((pf) => {
          let productFarm = { ...pf };
          productFarm.url = `/farms/${pf.farmId}`;
          return productFarm;
        });
      }

      return productItem;
    });

    return (
      <RelationBox className="mt-3">
        <TertiaryHeading>Other Products</TertiaryHeading>
        <div className="row">
          {relevants
            ? relevants.map((item, index) => {
                return (
                  <div
                    key={index}
                    className="col col-12 col-sm-6 col-md-6 col-lg-6 col-xl-4"
                  >
                    <ProductItem product={item} />
                  </div>
                );
              })
            : null}
        </div>
      </RelationBox>
    );
  };

  return (
    <Fragment>
      <Breadcrumb list={breadcrumbs} />
      <Detail product={product} breadcrumbs={breadcrumbs} />
      {renderRelevants(relevantLoading, relevantData)}
    </Fragment>
  );
});
