﻿using HMUI;
using System;
using Zenject;
using IPA.Utilities;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using UnityEngine;
using System.Threading.Tasks;

namespace Enhancements.UI
{
    [ViewDefinition("Enhancements.Views.settings-navigation-view.bsml")]
    [HotReload(RelativePathToLayout = @"..\Views\settings-navigation-view.bsml")]
    public class XSettingsNavigationController : BSMLAutomaticViewController
    {
        public event Action<string, int> DidSelectSettingOption;

        [UIComponent("list")]
        protected CustomListTableData tableList;

        private XLoader _loader;

        [Inject]
        public void Construct(XLoader loader)
        {
            _loader = loader;
        }

        [UIAction("option-selected")]
        protected void OptionSelected(TableView _, int id)
        {
            DidSelectSettingOption?.Invoke(tableList.data[id].text.Split('\n')[0], id);
        }

        [UIAction("#post-parse")]
        protected async Task Parsed()
        {
            tableList.data.AddRange(new CustomListTableData.CustomCellInfo[]
            {
                new CustomListTableData.CustomCellInfo
                (
                    "Changelog\n",
                    "The changelog for Enhancements",
                    _loader.GetIcon("changelog")
                    
                ),
                new CustomListTableData.CustomCellInfo
                (
                    "Clock\n",
                    "Modify the Clock",
                    _loader.GetIcon("clock")
                ),
                new CustomListTableData.CustomCellInfo
                (
                    "Timers\n",
                    "Create Reminders In Game!",
                    _loader.GetIcon("timer")
                ),
                new CustomListTableData.CustomCellInfo
                (
                    "Breaktime\n",
                    "Get Information During Song Breaks!",
                    _loader.GetIcon("breaktime")
                ),
                new CustomListTableData.CustomCellInfo
                (
                    "Volume\n",
                    "Change Specific Volume Settings",
                    _loader.GetIcon("volume")
                ),
                new CustomListTableData.CustomCellInfo
                (
                    "Mini Settings\n",
                    "Miscellaneous Tweaks",
                    _loader.GetIcon("settings")
                )
            });
            tableList.tableView.ReloadData();
            // Monkey Patch
            foreach (var imageView in tableList.GetComponentsInChildren<ImageView>())
            {
                imageView.SetField("_skew", 0f);
                imageView.SetVerticesDirty();
                if (imageView.gameObject.name == "Artwork")
                {
                    imageView.transform.localScale = new Vector3(0.55f, 0.65f, 0.55f);
                    //imageView.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
                }
            }
            SelectFirstCell();
            await SiraUtil.Utilities.PauseChamp;
            var rt = (tableList.transform as RectTransform);
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, 25);
        }

        public void SelectFirstCell()
        {
            tableList?.tableView.SelectCellWithIdx(0);
        }
        protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
        {
            base.DidDeactivate(removedFromHierarchy, screenSystemDisabling);
        }
    }
}