﻿using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class FaceunityWorker : MonoBehaviour
{
    public class CFaceUnityCoefficientSet
    {
        public float[] m_data;
        public int[] m_data_int;
        public GCHandle m_handle;
        public IntPtr m_addr;
        public string m_name;
        public int m_addr_size;
        public int m_faceId = 0;
        public int m_datatype = 0;  //0为float，1为int

        public CFaceUnityCoefficientSet(string s, int num, int faceId = 0,int datatype=0)
        {
            m_name = s;
            m_addr_size = num;
            m_faceId = faceId;
            m_datatype = datatype;

            if (m_datatype == 0)
            {
                m_data = new float[m_addr_size];
                m_handle = GCHandle.Alloc(m_data, GCHandleType.Pinned);
                m_addr = m_handle.AddrOfPinnedObject();
            }
            else if (m_datatype == 1)
            {
                m_data_int = new int[m_addr_size];
                m_handle = GCHandle.Alloc(m_data_int, GCHandleType.Pinned);
                m_addr = m_handle.AddrOfPinnedObject();
            }
            else
            {
                Debug.LogError("CFaceUnityCoefficientSet Error! Unknown datatype");
                return;
            }
        }
        ~CFaceUnityCoefficientSet()
        {
            if (m_handle != null && m_handle.IsAllocated)
            {
                m_handle.Free();
                m_data = default(float[]);
                m_data_int = default(int[]);
            }
        }
        public void Update()
        {
            fu_GetFaceInfo(m_faceId, m_addr, m_addr_size, m_name);
        }

        public void Update(int num)
        {
            if(num!= m_addr_size)
            {
                if (m_handle != null && m_handle.IsAllocated)
                {
                    m_handle.Free();
                }
                m_addr_size = num;
                if (m_datatype == 0)
                {
                    m_data = new float[m_addr_size];
                    m_handle = GCHandle.Alloc(m_data, GCHandleType.Pinned);
                    m_addr = m_handle.AddrOfPinnedObject();
                }
                else if (m_datatype == 1)
                {
                    m_data_int = new int[m_addr_size];
                    m_handle = GCHandle.Alloc(m_data_int, GCHandleType.Pinned);
                    m_addr = m_handle.AddrOfPinnedObject();
                }
                else
                    return;
            }
            Update();
        }
    }

    // Unity editor doesn't unload dlls after 'preview'

    #region DllImport

    /////////////////////////////////////
    //native interfaces

    /**
	\brief Initialize and authenticate your SDK instance to the FaceUnity server, must be called exactly once before all other functions.
		The buffers should NEVER be freed while the other functions are still being called.
		You can call this function multiple times to "switch pointers".
	\param v3buf should point to contents of the "v3.bin" we provide
    \param v3buf_sz should point to num-of-bytes of the "v3.bin" we provide
	\param licbuf is the pointer to the authentication data pack we provide. You must avoid storing the data in a file.
		Normally you can just `#include "authpack.h"` and put `g_auth_package` here.
	\param licbuf_sz is the authenticafu_Cleartion data size, we use plain int to avoid cross-language compilation issues.
		Normally you can just `#include "authpack.h"` and put `sizeof(g_auth_package)` here.
	\return non-zero for success, zero for failure
	*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int fu_Setup(IntPtr v3buf, int v3buf_sz, IntPtr licbuf, int licbuf_sz);

#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int fu_GetNamaInited();

#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int fu_LoadExtendedARData(IntPtr databuf, int databuf_sz);

#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int fu_LoadAnimModel(IntPtr databuf, int databuf_sz);

#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int fu_LoadTongueModel(IntPtr databuf, int databuf_sz);

#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern void fu_SetExpressionCalibration(int enable);

#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    private static extern void fu_setItemDataFromPackage(IntPtr databuf, int databuf_sz);

#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int fu_getItemIdxFromPackage();

    /**
\brief Destroy an accessory item.
    This function MUST be called in the same GLES context / thread as the original fuCreateItemFromPackage.
\param item is the handle to be destroyed
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern void fu_DestroyItem(int itemid);

    /**
\brief Destroy all accessory items ever created.
    This function MUST be called in the same GLES context / thread as the original fuCreateItemFromPackage.
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern void fu_DestroyAllItems();

    /**
	\brief Render a list of items on top of a GLES texture or a memory buffer.
		This function needs a GLES 2.0+ context. 
		Render will do in PluginEvent fu_GetRenderEventFunc
	\param idxbuf points to the list of items
	\param idxbuf_sz is the number of items
	*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int fu_setItemIds(IntPtr idxbuf, int idxbuf_sz, IntPtr mask);//mask can be null

    /**
\brief Set the default rotationMode.
\param rotationMode is the default rotationMode to be set to, one of 0..3 should work.
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern void fu_SetDefaultRotationMode(int i);

    /**
\brief Get certificate permission code for modules
\param i - get i-th code, currently available for 0 and 1
\return The permission code
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int fu_GetModuleCode(int i);

#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int fu_SetASYNCTrackFace(int i);

#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int fu_SetRuningMode(int runningMode);//refer to FURuningMode 

    /**
\brief Create Tex For Item
\param item specifies the item
\param name is the tex name
\param value is the tex rgba buffer to be set ,use GCHandle to get ptr
\param width is the tex width
\param height is the tex height
\return zero for failure, non-zero for success
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int fu_CreateTexForItem(int itemid, [MarshalAs(UnmanagedType.LPStr)]string name, IntPtr value, int width, int height);

    /**
\brief Delete Tex For Item
\param item specifies the item
\param name is the parameter name
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int fu_DeleteTexForItem(int itemid, [MarshalAs(UnmanagedType.LPStr)]string name);

    /**
\brief Set an item parameter to a double value
\param item specifies the item
\param name is the parameter name
\param value is the parameter value to be set
\return zero for failure, non-zero for success
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int fu_ItemSetParamd(int itemid, [MarshalAs(UnmanagedType.LPStr)]string name, double value);

    /**
\brief Set an item parameter to a double array
\param item specifies the item
\param name is the parameter name
\param value points to an array of doubles
\param n specifies the number of elements in value
\return zero for failure, non-zero for success
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int fu_ItemSetParamdv(int itemid, [MarshalAs(UnmanagedType.LPStr)]string name, IntPtr value, int value_sz);

    /**
\brief Set an item parameter to a string value
\param item specifies the item
\param name is the parameter name
\param value is the parameter value to be set
\return zero for failure, non-zero for success
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int fu_ItemSetParams(int itemid, [MarshalAs(UnmanagedType.LPStr)]string name, [MarshalAs(UnmanagedType.LPStr)]string value);

    /**
\brief Set an item parameter to a string value
\param item specifies the item
\param name is the parameter name
\param value is the parameter value to be set
\return zero for failure, non-zero for success
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern double fu_ItemGetParamd(int itemid, [MarshalAs(UnmanagedType.LPStr)]string name);

    /**
\brief Get an item parameter as a string
\param item specifies the item
\param name is the parameter name
\param buf receives the string value
\param sz is the number of bytes available at buf
\return the length of the string value, or -1 if the parameter is not a string.
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int fu_ItemGetParams(int itemid, [MarshalAs(UnmanagedType.LPStr)]string name, [MarshalAs(UnmanagedType.LPStr)]byte[] buf, int buf_sz);

    /**
\brief Set the default orientation for face detection. The correct orientation would make the initial detection much faster.
\param rmode is the default orientation to be set to, one of 0..3 should work.
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern void fu_SetDefaultOrientation(int rmode);

#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern IntPtr fu_GetRenderEventFunc();

    /**
* if plugin inited
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int jc_part_inited();

    /**
* SetUseNatCam(1);
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern void SetUseNatCam(int enable);

    /**
* if true,Pause the render pipeline
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern void SetPauseRender(bool ifpause);

    /**
\brief Get the face tracking status
\return The number of valid faces currently being tracked
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int fu_IsTracking();

    /**
\brief Set the maximum number of faces we track. The default value is 1.
\param n is the new maximum number of faces to track
\return The previous maximum number of faces tracked
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int fu_SetMaxFaces(int num);

    /**
\brief Set the quality-performance tradeoff. 
\param quality is the new quality value. 
       It's a floating point number between 0 and 1.
       Use 0 for maximum performance and 1 for maximum quality.
       The default quality is 1 (maximum quality).
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern void fu_SetQualityTradeoff(float num);

    /**
\brief Get SDK version string
\return SDK version string in const char*
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern IntPtr fu_GetVersion(); // Marshal.PtrToStringAnsi(fu_GetVersion());

    /**
\brief Get system error, which causes system shutting down
\return System error code represents one or more errors	
	Error code can be checked against following bitmasks, non-zero result means certain error
	This interface is not a callback, needs to be called on every frame and check result, no cost
	Inside authentication error (NAMA_ERROR_BITMASK_AUTHENTICATION), meanings for each error code are listed below:
	1 failed to seed the RNG
	2 failed to parse the CA cert
	3 failed to connect to the server
	4 failed to configure TLS
	5 failed to parse the client cert
	6 failed to parse the client key
	7 failed to setup TLS
	8 failed to setup the server hostname
	9 TLS handshake failed
	10 TLS verification failed
	11 failed to send the request
	12 failed to read the response
	13 bad authentication response
	14 incomplete authentication palette info
	15 not inited yet
	16 failed to create a thread
	17 authentication package rejected
	18 void authentication data
	19 bad authentication package
	20 certificate expired
	21 invalid certificate
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int fu_GetSystemError();

    /**
\brief Interpret system error code
\param code - System error code returned by fuGetSystemError()
\return One error message from the code
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern IntPtr fu_GetSystemErrorString(int code); // Marshal.PtrToStringAnsi();

    /**
\brief Call this function when the GLES context has been lost and recreated.
    That isn't a normal thing, so this function could leak resources on each call.
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern void fu_OnDeviceLost();

    /**
\brief Call this function to reset the face tracker on camera switches
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern void fu_OnCameraChange();


    /**
\brief clear camera frame data
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern void ClearImages();

    /**
     * 翻转输入的纹理，仅对使用了natcam的安卓平台有效
     * natcam的安卓平台使用了SetDualInput，有些安卓平台nv21buf和tex的方向不一致，可以用这个接口设置tex的镜像。
     */
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int SetFlipTexMarkX(bool mark);

#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int SetFlipTexMarkY(bool mark);

    /**
\brief provide camera frame data
        flags: FU_ADM_FLAG_FLIP_X = 32;
               FU_ADM_FLAG_FLIP_Y = 64; 翻转只翻转道具渲染，并不会翻转整个图像
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int SetImage(IntPtr imgbuf,int flags, bool isbgra, int w, int h);

    /**
\brief provide camera frame data android nv21 and texture id
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int SetDualInput(IntPtr nv21buf, int texid, int flags, int w, int h);

    /**
\brief provide camera frame data android nv21,only support Android.
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int SetNV21Input(IntPtr nv21buf, int flags, int w, int h);

    /**
\brief provide camera frame data via texid
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int SetImageTexId(int texid, int flags, int w, int h);

    /**
\brief Enable internal Log
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern void fu_EnableLog(bool isenable);

    /**
\brief get Rendered texture id, can be recreated in unity
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int fu_GetNamaTextureId();

    /**
  \brief Get the unique identifier for each face during current tracking
    Lost face tracking will change the identifier, even for a quick retrack
  \param face_id is the id of face, index is smaller than which is set in fuSetMaxFaces
    If this face_id is x, it means x-th face currently tracking
  \return the unique identifier for each face
  */
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int fu_GetFaceIdentifier(int face_id);

    /**
\brief Scale the rendering perspectivity (focal length, or FOV)
   Larger scale means less projection distortion
   This scale should better be tuned offline, and set it only once in runtime
\param scale - default is 1.f, keep perspectivity invariant
   <= 0.f would be treated as illegal input and discard	
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern void fu_SetFocalLengthScale(float scale);

#if !UNITY_IOS
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    private static extern void RegisterDebugCallback(DebugCallback callback);
#endif

#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int fu_GetFaceInfo(int face_id, IntPtr ret, int szret, [MarshalAs(UnmanagedType.LPStr)]string name);

    /**
     * FURuningMode为FU_Mode_RenderItems的时候，加载EnableTongueForUnity.bytes，才能开启舌头跟踪。
     * FURuningMode为FU_Mode_TrackFace的时候，调用fu_SetTongueTracking(1)，才能开启舌头跟踪。注意，每次切换到FU_Mode_TrackFace之后都需要设置一次！！！
\brief Turn on or turn off Tongue Tracking, used in trackface.
\param enable > 0 means turning on, enable <= 0 means turning off
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int fu_SetTongueTracking(int enable);

    /**
\brief Set a face detector parameter.
\param detector is the detector context, currently it is allowed to set to NULL, i.e., globally set all contexts.
\param name is the parameter name, it can be:
	"use_new_cnn_detection": 1 if the new cnn-based detection method is used, 0 else
	"other_face_detection_frame_step": if one face already exists, then we detect other faces not each frame, but with a step
	if use_new_cnn_detection == 1, then
		"min_facesize_small", int[default=18]: minimum size to detect a small face; must be called **BEFORE** fuSetup
		"min_facesize_big", int[default=27]: minimum size to detect a big face; must be called **BEFORE** fuSetup
		"small_face_frame_step", int[default=5]: the frame step to detect a small face; it is time cost, thus we do not detect each frame
		"use_cross_frame_speedup", int[default=0]: perform a half-cnn inference each frame to speedup
	else
		"scaling_factor": the scaling across image pyramids, default 1.2f
		"step_size": the step of each sliding window, default 2.f
		"size_min": minimal face supported on 640x480 image, default 50.f
		"size_max": maximal face supported on 640x480 image, default is a large value
		"min_neighbors": algorithm internal, default 3.f
		"min_required_variance": algorithm internal, default 15.f
		"is_mono": specifies the input is monocular or BGRA 
\param value points to the new parameter value, e.g., 
	float scaling_factor=1.2f;
	dde_facedet_set(ctx, "scaling_factor", &scaling_factor);
	float size_min=100.f;
	dde_facedet_set(ctx, "size_min", &size_min);
*/
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("faceplugin", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int fu_SetFaceDetParam([MarshalAs(UnmanagedType.LPStr)]string name, IntPtr value);

    #endregion

    /**
 \brief Create an accessory item from a binary package, you can discard the data after the call.
     This function MUST be called in the same GLES context / thread as fuRenderItems.
 \param data is the pointer to the data
 \param sz is the data size, we use plain int to avoid cross-language compilation issues
 \return an integer handle represefu_Clearnting the item
 */
    public static IEnumerator fu_CreateItemFromPackage(IntPtr databuf, int databuf_sz)
    {
        fu_setItemDataFromPackage(databuf, databuf_sz);
        //GL.IssuePluginEvent(fu_GetRenderEventFunc(), 101);
        yield return Util._endOfFrame;
        yield return Util._endOfFrame;   //等待道具异步加载完毕
    }

    public enum FURuningMode
    {
        FU_Mode_None = 0,
        FU_Mode_RenderItems, //face tracking and render item (beautify is one type of item) ,item means 'daoju'
        FU_Mode_Beautification,//non face tracking, beautification only.
        FU_Mode_Masked,//when tracking multi-people, different perple　can use different item, give mask in function fu_setItemIds  
        FU_Mode_TrackFace//tracking face only, get face infomation, but do not render item.it's very fast.
    };

    public static FURuningMode currentMode = FURuningMode.FU_Mode_None;

    public static void SetRunningMode(FURuningMode mode)
    {
        currentMode = mode;
        fu_SetRuningMode((int)mode);
    }

    ///////////////////////////////
    //public int m_camera_native_texid = 0;
    //singleton checks
    public static FaceunityWorker instance = null;
    void Awake()
    {
        //singleton checks
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

    }
    ///////////////////////////////
    //persistent data, DO NOT EVER FREE ANY OF THESE!
    //we must keep the GC handles to keep the arrays pinned to the same addresses
    [HideInInspector]
    public bool m_plugin_inited = false;

    public int MAXFACE = 1;
    public bool EnableExpressionLoop = true;
    int MaxExpression = 0;
    public string LICENSE = "";

    [HideInInspector]
    public int m_need_update_facenum = 0;
    public List<CFaceUnityCoefficientSet> m_translation = new List<CFaceUnityCoefficientSet>();//("translation", 3); //3D translation of face in camera space - 3 float
    public List<CFaceUnityCoefficientSet> m_rotation = new List<CFaceUnityCoefficientSet>();//("rotation", 4); //rotation quaternion - 4 float
    public List<CFaceUnityCoefficientSet> m_rotation_mode = new List<CFaceUnityCoefficientSet>();//("rotation_mode", 1); //the relative orientaion of face agains phone, 0-3 - 1 float
    //public List<CFaceUnityCoefficientSet> m_expression = new List<CFaceUnityCoefficientSet>();//("expression", 46);
    public List<CFaceUnityCoefficientSet> m_expression_with_tongue = new List<CFaceUnityCoefficientSet>();//("expression_with_tongue", 56);
    //public List<CFaceUnityCoefficientSet> m_landmarks = new List<CFaceUnityCoefficientSet>();//("landmarks",75*2); //2D landmarks coordinates in image space - 75*2 float
    //public List<CFaceUnityCoefficientSet> m_landmarks_ar = new List<CFaceUnityCoefficientSet>();//("landmarks_ar",75*3); //3D landmarks coordinates in camera space - 75*3 float
    //public List<CFaceUnityCoefficientSet> m_projection_matrix = new List<CFaceUnityCoefficientSet>();//("projection_matrix",16); //the transform matrix from camera space to image space - 16 float
    //public List<CFaceUnityCoefficientSet> m_eye_rotation = new List<CFaceUnityCoefficientSet>();//("eye_rotation",4); //eye rotation quaternion - 4 float
    //public List<CFaceUnityCoefficientSet> m_face_rect = new List<CFaceUnityCoefficientSet>();//("face_rect",4); //the rectangle of tracked face in image space, (xmin,ymin,xmax,ymax) - 4 float
    //public List<CFaceUnityCoefficientSet> m_failure_rate = new List<CFaceUnityCoefficientSet>();//("failure_rate",1); //the failure rate of face tracking, the less the more confident about tracking result - 1 float
    public List<CFaceUnityCoefficientSet> m_pupil_pos = new List<CFaceUnityCoefficientSet>();//("pupil_pos", 4);
    public List<CFaceUnityCoefficientSet> m_focallength = new List<CFaceUnityCoefficientSet>();//("focal_length", 1);

    //public List<CFaceUnityCoefficientSet> m_armesh_vertex_num = new List<CFaceUnityCoefficientSet>();
    //public List<CFaceUnityCoefficientSet> m_armesh_vertices = new List<CFaceUnityCoefficientSet>();
    //public List<CFaceUnityCoefficientSet> m_armesh_uvs = new List<CFaceUnityCoefficientSet>();
    //public List<CFaceUnityCoefficientSet> m_armesh_face_num = new List<CFaceUnityCoefficientSet>();
    //public List<CFaceUnityCoefficientSet> m_armesh_faces = new List<CFaceUnityCoefficientSet>();

    public static float FocalLengthScale = 1f;

    public static event EventHandler OnInitOK;
    private delegate void DebugCallback(string message);

    bool isfirstLoop = true;
    GCHandle m_licdata_handle;
    GCHandle m_v3data_handle;
    GCHandle ardata_exdata_handle;
    GCHandle anim_model_handle;
    GCHandle tongue_handle;

    ///////////////////////////////
    void InitCFaceUnityCoefficientSet(int maxface)
    {
        if (MaxExpression < maxface)
            for (int i = MaxExpression; i < maxface; i++)
            {
                m_translation.Add(new CFaceUnityCoefficientSet("translation", 3, i));
                m_rotation.Add(new CFaceUnityCoefficientSet("rotation", 4, i));
                m_rotation_mode.Add(new CFaceUnityCoefficientSet("rotation_mode", 1, i));
                //m_expression.Add(new CFaceUnityCoefficientSet("expression", 46, i));
                m_expression_with_tongue.Add(new CFaceUnityCoefficientSet("expression_with_tongue", 56, i));
                m_pupil_pos.Add(new CFaceUnityCoefficientSet("pupil_pos", 4, i));
                m_focallength.Add(new CFaceUnityCoefficientSet("focal_length", 1, i));
                //m_landmarks.Add(new CFaceUnityCoefficientSet("landmarks", 75 * 2, i));

                //m_armesh_vertex_num.Add(new CFaceUnityCoefficientSet("armesh_vertex_num", 1, i, 1));
                //m_armesh_vertices.Add(new CFaceUnityCoefficientSet("armesh_vertices", 1, i));   //这个长度值需要动态更新
                //m_armesh_uvs.Add(new CFaceUnityCoefficientSet("armesh_uvs", 1, i));
                //m_armesh_face_num.Add(new CFaceUnityCoefficientSet("armesh_face_num", 1, i, 1));
                //m_armesh_faces.Add(new CFaceUnityCoefficientSet("armesh_faces", 1, i, 1));
            }
        else if (MaxExpression > maxface)
            for (int i = maxface; i < MaxExpression; i++)
            {
                m_translation.RemoveAt(i);
                m_rotation.RemoveAt(i);
                m_rotation_mode.RemoveAt(i);
                //m_expression.RemoveAt(i);
                m_expression_with_tongue.RemoveAt(i);
                m_pupil_pos.RemoveAt(i);
                m_focallength.RemoveAt(i);
                //m_landmarks.RemoveAt(i);

                //m_armesh_vertex_num.RemoveAt(i);
                //m_armesh_vertices.RemoveAt(i);
                //m_armesh_uvs.RemoveAt(i);
                //m_armesh_face_num.RemoveAt(i);
                //m_armesh_faces.RemoveAt(i);
            }
        MaxExpression = maxface;
    }

    //working methods
    IEnumerator Start()
    {
        if (EnvironmentCheck())
        {
            Debug.Log("jc_part_inited:   " + jc_part_inited());
            if (m_plugin_inited == false)
            {
                Debug.LogFormat("FaceunityWorker Init");
#if UNITY_EDITOR && !UNITY_IOS
                RegisterDebugCallback(new DebugCallback(DebugMethod));
#endif
                fu_EnableLog(false);
                ClearImages();

                //fu_Setup init nama sdk
                if (jc_part_inited() == 0)  //防止Editor下二次Play导致崩溃的bug
                {
                    //load license data
                    if (LICENSE == null || LICENSE == "")
                    {
                        Debug.LogError("LICENSE is null! please paste the license data to the TextField named \"LICENSE\" in FaceunityWorker");
                    }
                    else
                    {
                        sbyte[] m_licdata_bytes;
                        string[] sbytes = LICENSE.Split(',');
                        if (sbytes.Length <= 7)
                        {
                            Debug.LogError("License Format Error");
                        }
                        else
                        {
                            m_licdata_bytes = new sbyte[sbytes.Length];
                            Debug.LogFormat("length:{0}", sbytes.Length);
                            for (int i = 0; i < sbytes.Length; i++)
                            {
                                //Debug.Log(sbytes[i]);
                                m_licdata_bytes[i] = sbyte.Parse(sbytes[i]);
                                //Debug.Log(m_licdata_bytes[i]);
                            }
                            if (m_licdata_handle.IsAllocated)
                                m_licdata_handle.Free();
                            m_licdata_handle = GCHandle.Alloc(m_licdata_bytes, GCHandleType.Pinned);
                            IntPtr licptr = m_licdata_handle.AddrOfPinnedObject();

                            //load nama sdk data
                            string fnv3 = Util.GetStreamingAssetsPath() + "/faceunity/v3.bytes";
                            WWW v3data = new WWW(fnv3);
                            yield return v3data;
                            byte[] m_v3data_bytes = v3data.bytes;
                            if (m_v3data_handle.IsAllocated)
                                m_v3data_handle.Free();
                            m_v3data_handle = GCHandle.Alloc(m_v3data_bytes, GCHandleType.Pinned); //pinned avoid GC
                            IntPtr v3ptr = m_v3data_handle.AddrOfPinnedObject(); //pinned addr

                            fu_Setup(v3ptr, m_v3data_bytes.Length, licptr, sbytes.Length); //要查看license是否有效请打开插件log（fu_EnableLog(true);）

                            m_plugin_inited = true;
                        }
                    }
                }
                else
                {
                    fu_OnDeviceLost();  //清理残余，防止崩溃
                    m_plugin_inited = true;
                }

                if (m_plugin_inited == true)
                {
                    string tongue = Util.GetStreamingAssetsPath() + "/faceunity/tongue.bytes";    //舌头检测
                    WWW tonguedata = new WWW(tongue);
                    yield return tonguedata;
                    byte[] tongue_bytes = tonguedata.bytes;
                    if (tongue_handle.IsAllocated)
                        tongue_handle.Free();
                    tongue_handle = GCHandle.Alloc(tongue_bytes, GCHandleType.Pinned);
                    IntPtr tonguedataptr = tongue_handle.AddrOfPinnedObject();
                    fu_LoadTongueModel(tonguedataptr, tongue_bytes.Length);

                    fu_SetASYNCTrackFace(0);    //异步人脸跟踪，部分平台能提升性能，默认关闭
                    SetRunningMode(FURuningMode.FU_Mode_RenderItems);   //默认模式，随时可以改
                    SetUseNatCam(1);  //默认选项，仅安卓有效
                    fu_SetFocalLengthScale(FocalLengthScale);   //默认值是1
                    Debug.LogFormat("fu_SetFocalLengthScale({0})", FocalLengthScale);

                    if (OnInitOK != null)
                        OnInitOK(this, null);//触发初始化完成事件

                    //Debug.Log("错误：" + fu_GetSystemError() +","+ Marshal.PtrToStringAnsi(fu_GetSystemErrorString(fu_GetSystemError())));
                    Debug.Log("SDK Version:" + Marshal.PtrToStringAnsi(fu_GetVersion()));

                    yield return StartCoroutine("CallPluginAtEndOfFrames");
                }
            }
        }
        else
        {
            Debug.LogError("please check your Graphics API,this plugin only supports OpenGL!");
            yield return null;
        }
    }
    private IEnumerator CallPluginAtEndOfFrames()
    {
        while (true)
        {
            yield return Util._endOfFrame;
            ////////////////////////////////
            fu_SetMaxFaces(MAXFACE);
            GL.IssuePluginEvent(fu_GetRenderEventFunc(), 1);// cal for sdk render
            if (isfirstLoop)
            {
                isfirstLoop = false;
                if (m_licdata_handle.IsAllocated)
                    m_licdata_handle.Free();
                if (m_v3data_handle.IsAllocated)
                    m_v3data_handle.Free();
                if (ardata_exdata_handle.IsAllocated)
                    ardata_exdata_handle.Free();
                if (anim_model_handle.IsAllocated)
                    anim_model_handle.Free();
                if (tongue_handle.IsAllocated)
                    tongue_handle.Free();
            }
            if (EnableExpressionLoop)
            {
                if (MaxExpression != MAXFACE)
                    InitCFaceUnityCoefficientSet(MAXFACE);
                //only update other stuff when there is new data
                int num = fu_IsTracking();
                m_need_update_facenum = num < MAXFACE ? num : MAXFACE;
                for (int i = 0; i < m_need_update_facenum; i++)
                {
                    //m_armesh_vertex_num[i].Update();
                    //m_armesh_vertices[i].Update(m_armesh_vertex_num[i].m_data_int[0] * 3);
                    //m_armesh_uvs[i].Update(m_armesh_vertex_num[i].m_data_int[0] * 2);
                    //m_armesh_face_num[i].Update();
                    //m_armesh_faces[i].Update(m_armesh_face_num[i].m_data_int[0] * 3);

                    m_translation[i].Update();
                    m_rotation[i].Update();
                    m_rotation_mode[i].Update();
                    //m_landmarks[i].Update();
                    m_pupil_pos[i].Update();
                    m_focallength[i].Update();
                    ////////////////////////

                    m_expression_with_tongue[i].Update();
                    m_expression_with_tongue[i].m_data[6] = m_expression_with_tongue[i].m_data[7] = m_pupil_pos[i].m_data[0];
                    m_expression_with_tongue[i].m_data[10] = m_expression_with_tongue[i].m_data[11] = -m_pupil_pos[i].m_data[0];
                    m_expression_with_tongue[i].m_data[12] = m_expression_with_tongue[i].m_data[13] = m_pupil_pos[i].m_data[1];
                    m_expression_with_tongue[i].m_data[4] = m_expression_with_tongue[i].m_data[5] = -m_pupil_pos[i].m_data[1];

                    //m_expression[i].Update();
                    //m_expression[i].m_data[6] = m_expression[i].m_data[7] = m_pupil_pos[i].m_data[0];
                    //m_expression[i].m_data[10] = m_expression[i].m_data[11] = -m_pupil_pos[i].m_data[0];
                    //m_expression[i].m_data[12] = m_expression[i].m_data[13] = m_pupil_pos[i].m_data[1];
                    //m_expression[i].m_data[4] = m_expression[i].m_data[5] = -m_pupil_pos[i].m_data[1];
                }
            }
        }
    }

    private static void DebugMethod(string message)
    {
        Debug.Log("From Dll: " + message);
    }

    private void OnApplicationQuit()
    {
        if (m_licdata_handle.IsAllocated)
            m_licdata_handle.Free();
        if (m_v3data_handle.IsAllocated)
            m_v3data_handle.Free();
        if (ardata_exdata_handle.IsAllocated)
            ardata_exdata_handle.Free();
        if (anim_model_handle.IsAllocated)
            anim_model_handle.Free();
        if (tongue_handle.IsAllocated)
            tongue_handle.Free();
        if (m_plugin_inited == true)
            fu_OnDeviceLost();
        ClearImages();
    }

    bool EnvironmentCheck()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLCore
            || SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGL2)
            return true;
        else
            return false;
#elif UNITY_STANDALONE_OSX||UNITY_EDITOR_OSX
        if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLCore
            || SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGL2)
            return true;
        else
            return false;
#elif UNITY_ANDROID
        if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES3
            || SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES2)
            return true;
        else
            return false;
#elif UNITY_IOS
        if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES3
            || SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES2)
            return true;
        else
            return false;
#endif
    }
}
