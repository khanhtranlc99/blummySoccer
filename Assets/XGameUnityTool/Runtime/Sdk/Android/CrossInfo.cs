using System;
using UnityEngine.Scripting;

namespace XGame
{   
    /**互推信息 */
    [Serializable] 
    [Preserve] 
    public class CrossInfo
    {
        /**展示类型 */
        public string type;
        /**游戏名称 */ 
        public string name; 
        /**游戏包名 */
        public string package; 
        /**游戏图标 */
        public string pic;
        /**游戏描述 */
        public string desc; 
        /**游戏权重 */
        public int weight;
        /**是否已安装 */
        public bool isInstall;

    }
    
    public enum CrossActionType
    {
        Show,
        Click,
    }
    
}