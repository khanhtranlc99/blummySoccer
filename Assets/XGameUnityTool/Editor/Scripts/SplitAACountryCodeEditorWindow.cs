using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XGame
{
    public class SplitAACountryCodeEditorWindow : EditorWindow
    {
        //每行个数
        private const int COUNT_PER_ROW = 4;

        //元素宽高
        private const float ELEMENT_HEIGHT = 22;
        private const float ELEMENT_WIDTH = 400;
        private static SplitAAOption _option;
        private string inputTxt;
        private Vector2 _scroll;
        private bool _closedFlag = false;

        public static void Open(SplitAAOption option)
        {
            _option = option;
            var window = GetWindow<SplitAACountryCodeEditorWindow>();
            window.minSize = new Vector2(ELEMENT_WIDTH * COUNT_PER_ROW, 500);
            window.maxSize = new Vector2(ELEMENT_WIDTH * COUNT_PER_ROW, Screen.height);
            window.titleContent = new GUIContent("List of countries/regions");
            window.Show();
        }


        private void OnGUI()
        {
            if (_option == null)
            {
                Close();
            }

            var rect = new Rect(0, 0, position.width, position.height);
            var lastRect = rect;

            var rectCurrentLab = new Rect(rect);
            rectCurrentLab.height = Math.Max(100,(float)(_option.Countries.Count * 1.3));
            GUI.TextArea(rectCurrentLab, $"Currently selecting {_option.Countries.Count} options. {_option.ShowCodeDesc()}");

            lastRect = rectCurrentLab;
            var rectSearchBar = new Rect(lastRect);
            rectSearchBar.height = 22;
            rectSearchBar.y = lastRect.yMax;
            var searchBarClearBtnRect = new Rect(rectSearchBar);
            searchBarClearBtnRect.width = 22;
            searchBarClearBtnRect.x = rectSearchBar.xMax - searchBarClearBtnRect.width;
            rectSearchBar.width -= searchBarClearBtnRect.width;
            var center = searchBarClearBtnRect.center;
            searchBarClearBtnRect.width = 16;
            searchBarClearBtnRect.height = 16;
            searchBarClearBtnRect.center = center;

            inputTxt = EditorGUI.TextField(rectSearchBar, "", inputTxt);
            if (GUI.Button(searchBarClearBtnRect, "X"))
            {
                inputTxt = string.Empty;
                GUI.FocusControl("");
                Event.current.Use();
            }

            lastRect = rectSearchBar;
            var rectToolBar = new Rect(lastRect);
            rectToolBar.y = lastRect.yMax;
            GUILayout.BeginArea(rectToolBar);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Select All", GUILayout.Width(80)))
            {
                _option.OpenAll();
            }

            if (GUILayout.Button("Deselect all", GUILayout.Width(80)))
            {
                _option.CloseAll();
            }

            GUILayout.EndHorizontal();
            GUILayout.EndArea();


            var countryNames = SplitAAOption.CountryNames;
            lastRect = rectToolBar;
            var rectScrollViewArea = new Rect(lastRect);
            rectScrollViewArea.height = rect.height - lastRect.yMax;
            rectScrollViewArea.y = lastRect.yMax;

            GUILayout.BeginArea(rectScrollViewArea);
            var viewHeight = Mathf.CeilToInt((float)countryNames.Count / COUNT_PER_ROW) * ELEMENT_HEIGHT;
            var rectScrollView = new Rect(0, 0, rect.width, rectScrollViewArea.height);
            var rectView = new Rect(0, 0, rect.width, viewHeight);
            _scroll = GUI.BeginScrollView(rectScrollView, _scroll, rectView);
            var index = 0;
            var countries = _option.Countries;
            var contain = new Dictionary<CountyCode, object>();
            foreach (var country in countries)
            {
                contain[country] = null;
            }


            foreach (var country in countryNames)
            {
                bool isMatch = true;
                if (!string.IsNullOrEmpty(inputTxt))
                {
                    var searchMatch = inputTxt.ToLower();
                    isMatch = country.Key.ToString().ToLower().Contains(searchMatch) ||
                              country.Value.Contains(searchMatch);
                }

                if (!isMatch)
                {
                    continue;
                }


                bool isOn = contain.ContainsKey(country.Key);

                var y = index / COUNT_PER_ROW * ELEMENT_HEIGHT;
                var x = index % COUNT_PER_ROW * ELEMENT_WIDTH;
                var rectElement = new Rect(x, y, ELEMENT_WIDTH, ELEMENT_HEIGHT);

                var temColor = GUI.color;

                if (isOn)
                {
                    GUI.color = new Color(0.29f, 0.91f, 0.47f, 1f);
                }

                if (GUI.Button(rectElement, ""))
                {
                    if (isOn)
                    {
                        //关闭
                        _option.Close(country.Key);
                    }
                    else
                    {
                        //开启
                        _option.Open(country.Key);
                    }

                    Event.current.Use();
                    Repaint();
                }

                var rectElementName = new Rect(rectElement);
                rectElementName.width -= 4;
                rectElementName.center = rectElement.center;
                GUI.Label(rectElementName, $"{country.Key.ToString()} {country.Value}");

                GUI.color = temColor;
                index++;
            }

            GUI.EndScrollView();
            GUILayout.EndArea();
            Repaint();
            if (_closedFlag)
            {
                Close();
            }
        }


        private void OnLostFocus()
        {
            _closedFlag = true;
        }
    }
}