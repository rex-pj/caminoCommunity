USE Coco_IdentityDb

-- USER STATUS --
INSERT INTO dbo.[Status]
([Name], [Description])
VALUES
('New', 'New creating user')

INSERT INTO dbo.[Status]
([Name], [Description])
VALUES
('Pending', 'Waiting for verification')

INSERT INTO dbo.[Status]
([Name], [Description])
VALUES
('Actived', 'Active user')

INSERT INTO dbo.[Status]
([Name], [Description])
VALUES
('Reported', 'The user is being reported')

INSERT INTO dbo.[Status]
([Name], [Description])
VALUES
('Inactived', 'The user is Inactived')

INSERT INTO dbo.[Status]
([Name], [Description])
VALUES
('Blocked', 'The user is being blocked')

-- USER ROLE --
INSERT INTO dbo.[Role]
([Name], [Description])
VALUES
('Admin', 'The administrator')

INSERT INTO dbo.[Role]
([Name], [Description])
VALUES
('Moderator', 'The moderator')

INSERT INTO dbo.[Role]
([Name], [Description])
VALUES
('Approver', 'The approver')

-- GENDER --
INSERT INTO dbo.Gender
([Name])
VALUES
('Male')

INSERT INTO dbo.Gender
([Name])
VALUES
('Female')

INSERT INTO dbo.Gender
([Name])
VALUES
('Undefined')

-- COUNTRY --
SET IDENTITY_INSERT [dbo].[Country] ON 
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (2, N'AFG', N'Afghanistan')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (3, N'ALA', N'Åland Islands')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (4, N'ALB', N'Albania')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (5, N'DZA', N'Algeria')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (6, N'ASM', N'American Samoa')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (7, N'AND', N'Andorra')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (8, N'AGO', N'Angola')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (9, N'AIA', N'Anguilla')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (10, N'ATA', N'Antarctica')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (11, N'ATG', N'Antigua and Barbuda')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (12, N'ARG', N'Argentina')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (13, N'ARM', N'Armenia')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (14, N'ABW', N'Aruba')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (15, N'AUS', N'Australia')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (16, N'AUT', N'Austria')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (17, N'AZE', N'Azerbaijan')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (18, N'BHS', N'Bahamas')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (19, N'BHR', N'Bahrain')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (20, N'BGD', N'Bangladesh')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (21, N'BRB', N'Barbados')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (22, N'BLR', N'Belarus')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (23, N'BEL', N'Belgium')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (24, N'BLZ', N'Belize')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (25, N'BEN', N'Benin')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (26, N'BMU', N'Bermuda')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (27, N'BTN', N'Bhutan')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (28, N'BOL', N'Bolivia (Plurinational State of)')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (29, N'BES', N'Bonaire, Sint Eustatius and Saba')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (30, N'BIH', N'Bosnia and Herzegovina')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (31, N'BWA', N'Botswana')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (32, N'BVT', N'Bouvet Island')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (33, N'BRA', N'Brazil')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (34, N'IOT', N'British Indian Ocean Territory')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (35, N'BRN', N'Brunei Darussalam')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (36, N'BGR', N'Bulgaria')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (37, N'BFA', N'Burkina Faso')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (38, N'BDI', N'Burundi')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (39, N'CPV', N'Cabo Verde')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (40, N'KHM', N'Cambodia')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (41, N'CMR', N'Cameroon')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (42, N'CAN', N'Canada')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (43, N'CYM', N'Cayman Islands')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (44, N'CAF', N'Central African Republic')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (45, N'TCD', N'Chad')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (46, N'CHL', N'Chile')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (47, N'CHN', N'China')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (48, N'CXR', N'Christmas Island')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (49, N'CCK', N'Cocos (Keeling) Islands')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (50, N'COL', N'Colombia')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (51, N'COM', N'Comoros')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (52, N'COG', N'Congo')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (53, N'COD', N'Congo, Democratic Republic of the')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (54, N'COK', N'Cook Islands')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (55, N'CRI', N'Costa Rica')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (56, N'CIV', N'Côte d''Ivoire')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (57, N'HRV', N'Croatia')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (58, N'CUB', N'Cuba')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (59, N'CUW', N'Curaçao')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (60, N'CYP', N'Cyprus')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (61, N'CZE', N'Czechia')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (62, N'DNK', N'Denmark')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (63, N'DJI', N'Djibouti')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (64, N'DMA', N'Dominica')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (65, N'DOM', N'Dominican Republic')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (66, N'ECU', N'Ecuador')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (67, N'EGY', N'Egypt')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (68, N'SLV', N'El Salvador')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (69, N'GNQ', N'Equatorial Guinea')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (70, N'ERI', N'Eritrea')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (71, N'EST', N'Estonia')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (72, N'SWZ', N'Eswatini')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (73, N'ETH', N'Ethiopia')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (74, N'FLK', N'Falkland Islands (Malvinas)')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (75, N'FRO', N'Faroe Islands')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (76, N'FJI', N'Fiji')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (77, N'FIN', N'Finland')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (78, N'FRA', N'France')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (79, N'GUF', N'French Guiana')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (80, N'PYF', N'French Polynesia')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (81, N'ATF', N'French Southern Territories')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (82, N'GAB', N'Gabon')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (83, N'GMB', N'Gambia')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (84, N'GEO', N'Georgia')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (85, N'DEU', N'Germany')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (86, N'GHA', N'Ghana')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (87, N'GIB', N'Gibraltar')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (88, N'GRC', N'Greece')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (89, N'GRL', N'Greenland')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (90, N'GRD', N'Grenada')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (91, N'GLP', N'Guadeloupe')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (92, N'GUM', N'Guam')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (93, N'GTM', N'Guatemala')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (94, N'GGY', N'Guernsey')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (95, N'GIN', N'Guinea')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (96, N'GNB', N'Guinea-Bissau')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (97, N'GUY', N'Guyana')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (98, N'HTI', N'Haiti')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (99, N'HMD', N'Heard Island and McDonald Islands')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (100, N'VAT', N'Holy See')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (101, N'HND', N'Honduras')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (102, N'HKG', N'Hong Kong')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (103, N'HUN', N'Hungary')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (104, N'ISL', N'Iceland')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (105, N'IND', N'India')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (106, N'IDN', N'Indonesia')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (107, N'IRN', N'Iran (Islamic Republic of)')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (108, N'IRQ', N'Iraq')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (109, N'IRL', N'Ireland')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (110, N'IMN', N'Isle of Man')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (111, N'ISR', N'Israel')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (112, N'ITA', N'Italy')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (113, N'JAM', N'Jamaica')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (114, N'JPN', N'Japan')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (115, N'JEY', N'Jersey')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (116, N'JOR', N'Jordan')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (117, N'KAZ', N'Kazakhstan')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (118, N'KEN', N'Kenya')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (119, N'KIR', N'Kiribati')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (120, N'PRK', N'Korea (Democratic People''s Republic of)')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (121, N'KOR', N'Korea, Republic of')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (122, N'KWT', N'Kuwait')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (123, N'KGZ', N'Kyrgyzstan')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (124, N'LAO', N'Lao People''s Democratic Republic')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (125, N'LVA', N'Latvia')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (126, N'LBN', N'Lebanon')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (127, N'LSO', N'Lesotho')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (128, N'LBR', N'Liberia')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (129, N'LBY', N'Libya')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (130, N'LIE', N'Liechtenstein')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (131, N'LTU', N'Lithuania')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (132, N'LUX', N'Luxembourg')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (133, N'MAC', N'Macao')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (134, N'MDG', N'Madagascar')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (135, N'MWI', N'Malawi')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (136, N'MYS', N'Malaysia')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (137, N'MDV', N'Maldives')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (138, N'MLI', N'Mali')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (139, N'MLT', N'Malta')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (140, N'MHL', N'Marshall Islands')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (141, N'MTQ', N'Martinique')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (142, N'MRT', N'Mauritania')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (143, N'MUS', N'Mauritius')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (144, N'MYT', N'Mayotte')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (145, N'MEX', N'Mexico')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (146, N'FSM', N'Micronesia (Federated States of)')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (147, N'MDA', N'Moldova, Republic of')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (148, N'MCO', N'Monaco')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (149, N'MNG', N'Mongolia')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (150, N'MNE', N'Montenegro')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (151, N'MSR', N'Montserrat')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (152, N'MAR', N'Morocco')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (153, N'MOZ', N'Mozambique')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (154, N'MMR', N'Myanmar')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (155, N'NAM', N'Namibia')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (156, N'NRU', N'Nauru')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (157, N'NPL', N'Nepal')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (158, N'NLD', N'Netherlands')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (159, N'NCL', N'New Caledonia')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (160, N'NZL', N'New Zealand')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (161, N'NIC', N'Nicaragua')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (162, N'NER', N'Niger')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (163, N'NGA', N'Nigeria')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (164, N'NIU', N'Niue')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (165, N'NFK', N'Norfolk Island')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (166, N'MKD', N'North Macedonia')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (167, N'MNP', N'Northern Mariana Islands')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (168, N'NOR', N'Norway')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (169, N'OMN', N'Oman')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (170, N'PAK', N'Pakistan')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (171, N'PLW', N'Palau')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (172, N'PSE', N'Palestine, State of')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (173, N'PAN', N'Panama')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (174, N'PNG', N'Papua New Guinea')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (175, N'PRY', N'Paraguay')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (176, N'PER', N'Peru')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (177, N'PHL', N'Philippines')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (178, N'PCN', N'Pitcairn')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (179, N'POL', N'Poland')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (180, N'PRT', N'Portugal')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (181, N'PRI', N'Puerto Rico')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (182, N'QAT', N'Qatar')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (183, N'REU', N'Réunion')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (184, N'ROU', N'Romania')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (185, N'RUS', N'Russian Federation')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (186, N'RWA', N'Rwanda')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (187, N'BLM', N'Saint Barthélemy')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (188, N'SHN', N'Saint Helena, Ascension and Tristan da Cunha')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (189, N'KNA', N'Saint Kitts and Nevis')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (190, N'LCA', N'Saint Lucia')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (191, N'MAF', N'Saint Martin (French part)')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (192, N'SPM', N'Saint Pierre and Miquelon')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (193, N'VCT', N'Saint Vincent and the Grenadines')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (194, N'WSM', N'Samoa')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (195, N'SMR', N'San Marino')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (196, N'STP', N'Sao Tome and Principe')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (197, N'SAU', N'Saudi Arabia')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (198, N'SEN', N'Senegal')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (199, N'SRB', N'Serbia')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (200, N'SYC', N'Seychelles')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (201, N'SLE', N'Sierra Leone')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (202, N'SGP', N'Singapore')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (203, N'SXM', N'Sint Maarten (Dutch part)')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (204, N'SVK', N'Slovakia')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (205, N'SVN', N'Slovenia')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (206, N'SLB', N'Solomon Islands')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (207, N'SOM', N'Somalia')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (208, N'ZAF', N'South Africa')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (209, N'SGS', N'South Georgia and the South Sandwich Islands')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (210, N'SSD', N'South Sudan')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (211, N'ESP', N'Spain')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (212, N'LKA', N'Sri Lanka')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (213, N'SDN', N'Sudan')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (214, N'SUR', N'Suriname')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (215, N'SJM', N'Svalbard and Jan Mayen')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (216, N'SWE', N'Sweden')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (217, N'CHE', N'Switzerland')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (218, N'SYR', N'Syrian Arab Republic')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (219, N'TWN', N'Taiwan, Province of China')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (220, N'TJK', N'Tajikistan')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (221, N'TZA', N'Tanzania, United Republic of')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (222, N'THA', N'Thailand')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (223, N'TLS', N'Timor-Leste')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (224, N'TGO', N'Togo')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (225, N'TKL', N'Tokelau')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (226, N'TON', N'Tonga')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (227, N'TTO', N'Trinidad and Tobago')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (228, N'TUN', N'Tunisia')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (229, N'TUR', N'Turkey')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (230, N'TKM', N'Turkmenistan')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (231, N'TCA', N'Turks and Caicos Islands')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (232, N'TUV', N'Tuvalu')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (233, N'UGA', N'Uganda')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (234, N'UKR', N'Ukraine')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (235, N'ARE', N'United Arab Emirates')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (236, N'GBR', N'United Kingdom of Great Britain and Northern Ireland')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (237, N'USA', N'United States of America')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (238, N'UMI', N'United States Minor Outlying Islands')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (239, N'URY', N'Uruguay')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (240, N'UZB', N'Uzbekistan')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (241, N'VUT', N'Vanuatu')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (242, N'VEN', N'Venezuela (Bolivarian Republic of)')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (243, N'VNM', N'Viet Nam')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (244, N'VGB', N'Virgin Islands (British)')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (245, N'VIR', N'Virgin Islands (U.S.)')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (246, N'WLF', N'Wallis and Futuna')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (247, N'ESH', N'Western Sahara')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (248, N'YEM', N'Yemen')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (249, N'ZMB', N'Zambia')
GO
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (250, N'ZWE', N'Zimbabwe')
GO
SET IDENTITY_INSERT [dbo].[Country] OFF
GO
