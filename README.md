# Unity-SoundVisualizationUnity

**Unity 声音可视化网页交互项目**  

[点击前往在线演示界面](https://hannahhu.itch.io/singforme)

![项目截图](Screenshots/main_scene.png)

---

## 功能介绍

**实时声音交互**  
   - 打开麦克风权限，对着麦克风出声：  
     - 猴头模型随声音变形  
     - 底部圆圈随声音扩张  
     - 光源位置变化，使自制材质高光跟随音量变化  
---

## 使用说明

- GitHub 上只上传了：
  - 控制声音可视化实现的 **C# 脚本**  
  - WebGL 麦克风插件（MIT 授权，可免费使用）  
- **未上传**：
  - 场景文件、模型文件、Shader 文件  
- 想获取 Shader 或完整资源？欢迎通过邮箱联系我：2943433577@qq.com

> 友情提示：这个项目是我的课程作业，中间放了我的大学校徽。屏幕前的家人们如果有需要，也可以联系我，提出你的想法，我们可以换徽章、换模型、丰富背景~

---

## 致谢

- 感谢 [YouTube 教学视频](https://www.youtube.com/watch?v=uwCjzUTpR1E) 提供的 FFT 分析原始思路  
- Shader 制作灵感参考 [StylizedPaintShaderbreakdown](https://cyn-prod.com/stylized-paint-shader-breakdown) 博客，目前我在unity中使用纯shader+图片终于复刻成功了大约 30% 的效果，就已经把我迷的五迷三道了，非常漂亮，佩服大神  

---

## 技术信息

- **开发环境**：Unity6 urp
- **平台**：WebGL  
- **核心技术**：FFT 音频分析、自制 Shader、WebGL 麦克风交互  
- **未来改进**：计划加入更多材质变化、互动模型和光效优化  

---
