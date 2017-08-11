using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using RainMaker_Net;
using RaMaNetUnity;
using RaMaNet_VirusWarGameServer;

namespace RaMaNetUnity
{
    public class CReoteServerPeer : IPeer
    {
        public CUserToken token { get; private set; }
        WeakReference RaMaNet_eventManager;

        public CReoteServerPeer(CUserToken token)
        {
            this.token = token;
            this.token.set_peer(this);
        }

        public void set_eventmanager(CRaMaNetEventManager event_manager)
        {
            this.RaMaNet_eventManager = new WeakReference(event_manager);
        }

        // 메시지를 수신했을 대 호출된다.
        // 파라미터로 넘어온 버퍼는 워커 스레드에서 재사용 되므로 복사한 뒤 어플리케이션으로 넘겨준다.
        void IPeer.on_message(Const<byte[]> buffer)
        {
            // 버퍼를 복사한 뒤 CPacket 클래스로 감싼 뒤 넘겨준다.
            // CPacket 클래스 내부에서는 참조로만 들고 있는다.
            byte[] app_buffer = new byte[buffer.Value.Length];
            Array.Copy(buffer.Value, app_buffer, buffer.Value.Length);

            CPacket msg = new CPacket(app_buffer, this);
            (this.RaMaNet_eventManager.Target as CRaMaNetEventManager).enqueue_network_message(msg);
        }

        void IPeer.on_removed()
        {
            (this.RaMaNet_eventManager.Target as CRaMaNetEventManager).enqueue_network_event(NETWORK_EVENT.disconnected);
        }

        void IPeer.send(CPacket msg)
        {
            this.token.send(msg);
        }

        void IPeer.disconnect()
        {

        }

        void IPeer.process_user_operation(CPacket msg)
        {

        }
    }
}

